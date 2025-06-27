namespace MoodMusicProject.Models
{
    public class Mood
    {
        public int Id { get; set; }
        public string MoodName { get; set; }

        // 新增的导航属性
        public List<Song> Songs { get; set; }
    }
}