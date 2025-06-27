using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MoodMusicProject.Models;
using System;
using System.Collections.Generic;

namespace MoodMusicProject.Controllers
{
    public class MoodController : Controller
    {
        private readonly string connectionString = "server=localhost;database=MoodMusic;user=root;";

        public IActionResult Index() => View();

        public IActionResult Playlist(string mood, string song = null)
        {
            List<Song> songs = new List<Song>();

            if (string.IsNullOrWhiteSpace(mood))
            {
                ViewData["Mood"] = "default";
                return View("Playlist", songs);  // 空列表但避免崩溃
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT s.title, s.artist, s.file_path, s.youtube_url
                    FROM songs s
                    JOIN moods m ON s.mood_id = m.id
                    WHERE m.mood_name = @mood";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@mood", mood);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var url = reader["youtube_url"]?.ToString() ?? "";
                            if (!string.IsNullOrWhiteSpace(url)) // 忽略无效链接
                            {
                                songs.Add(new Song
                                {
                                    Title = reader["title"].ToString(),
                                    Artist = reader["artist"].ToString(),
                                    FilePath = reader["file_path"].ToString(),
                                    YoutubeUrl = url
                                });
                            }
                        }
                    }
                }
            }

            ViewData["Mood"] = mood;
            if (!string.IsNullOrWhiteSpace(song))
            {
                ViewData["Song"] = song;
            }

            return View("Playlist", songs);
        }

        [HttpPost]
        public IActionResult SubmitSuggestion([FromBody] Suggestion suggestion)
        {
            if (string.IsNullOrWhiteSpace(suggestion.Mood) ||
                string.IsNullOrWhiteSpace(suggestion.Title) ||
                string.IsNullOrWhiteSpace(suggestion.Artist) ||
                string.IsNullOrWhiteSpace(suggestion.YoutubeUrl))
                return BadRequest("Missing fields");

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var moodCmd = new MySqlCommand("INSERT IGNORE INTO moods (mood_name) VALUES (@mood)", conn);
                moodCmd.Parameters.AddWithValue("@mood", suggestion.Mood.ToLower());
                moodCmd.ExecuteNonQuery();

                var getIdCmd = new MySqlCommand("SELECT id FROM moods WHERE mood_name = @mood", conn);
                getIdCmd.Parameters.AddWithValue("@mood", suggestion.Mood.ToLower());
                int moodId = Convert.ToInt32(getIdCmd.ExecuteScalar());

                var insertSongCmd = new MySqlCommand(@"
                    INSERT INTO songs (title, artist, youtube_url, mood_id)
                    VALUES (@title, @artist, @url, @mood_id)", conn);
                insertSongCmd.Parameters.AddWithValue("@title", suggestion.Title);
                insertSongCmd.Parameters.AddWithValue("@artist", suggestion.Artist);
                insertSongCmd.Parameters.AddWithValue("@url", suggestion.YoutubeUrl);
                insertSongCmd.Parameters.AddWithValue("@mood_id", moodId);
                insertSongCmd.ExecuteNonQuery();
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetAllMoods()
        {
            List<string> moods = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT mood_name FROM moods", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        moods.Add(reader["mood_name"].ToString());
                }
            }
            return Json(moods);
        }

        [HttpGet]
        public IActionResult GetAllMoodDetails()
        {
            var result = new Dictionary<string, List<Song>>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(@"
                    SELECT s.id, s.title, s.artist, s.youtube_url, m.mood_name
                    FROM songs s
                    JOIN moods m ON s.mood_id = m.id", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string mood = reader["mood_name"].ToString();
                        if (!result.ContainsKey(mood))
                            result[mood] = new List<Song>();

                        result[mood].Add(new Song
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Title = reader["title"].ToString(),
                            Artist = reader["artist"].ToString(),
                            YoutubeUrl = reader["youtube_url"].ToString()
                        });
                    }
                }
            }

            return Json(result);
        }

        [HttpGet]
        public IActionResult SearchSong(string title)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(@"
                    SELECT m.mood_name
                    FROM songs s
                    JOIN moods m ON s.mood_id = m.id
                    WHERE LOWER(s.title) = @title", conn);

                cmd.Parameters.AddWithValue("@title", title.ToLower());

                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Json(new { success = true, mood = result.ToString() });
                }
            }

            return Json(new { success = false });
        }

        [HttpDelete]
        public IActionResult DeleteSong(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM songs WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                int rows = cmd.ExecuteNonQuery();
                return Json(new { success = rows > 0 });
            }
        }

        [HttpDelete]
        public IActionResult DeleteMood(string name)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var deleteSongs = new MySqlCommand(@"
                    DELETE s FROM songs s
                    JOIN moods m ON s.mood_id = m.id
                    WHERE m.mood_name = @name", conn);
                deleteSongs.Parameters.AddWithValue("@name", name);
                deleteSongs.ExecuteNonQuery();

                var deleteMood = new MySqlCommand("DELETE FROM moods WHERE mood_name = @name", conn);
                deleteMood.Parameters.AddWithValue("@name", name);
                deleteMood.ExecuteNonQuery();

                return Json(new { success = true });
            }
        }

        [HttpPost]
        public IActionResult UpdateSong([FromBody] Song song)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(@"
                    UPDATE songs 
                    SET title = @title, artist = @artist, youtube_url = @url 
                    WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@title", song.Title);
                cmd.Parameters.AddWithValue("@artist", song.Artist);
                cmd.Parameters.AddWithValue("@url", song.YoutubeUrl);
                cmd.Parameters.AddWithValue("@id", song.Id);
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdateMood([FromBody] Dictionary<string, string> data)
        {
            if (!data.ContainsKey("old") || !data.ContainsKey("updated"))
                return BadRequest();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE moods SET mood_name = @updated WHERE mood_name = @old", conn);
                cmd.Parameters.AddWithValue("@updated", data["updated"]);
                cmd.Parameters.AddWithValue("@old", data["old"]);
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true });
        }
    }
}