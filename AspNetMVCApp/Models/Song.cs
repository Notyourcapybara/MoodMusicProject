namespace MoodMusicProject.Models
{
    public class Song
    {
        public int Id { get; set; }           // ← 添加此字段用于识别和操作
        public string Title { get; set; }
        public string Artist { get; set; }
        public string FilePath { get; set; }
        public string YoutubeUrl { get; set; } = "";
    }
}