namespace MoodMusicProject.Models
{
    public class Song
    {
        public int Id { get; set; }           
        public string Title { get; set; }
        public string Artist { get; set; }
        public string FilePath { get; set; }
        public string YoutubeUrl { get; set; } = "";

        // 新增的外键字段
        public int MoodId { get; set; }

        // 新增的导航属性
        public Mood Mood { get; set; }
    }
}