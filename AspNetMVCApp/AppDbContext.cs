using Microsoft.EntityFrameworkCore;
using MoodMusicProject.Models;

namespace AspNetMVCApp
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mood> Moods { get; set; }
        public DbSet<Song> Songs { get; set; }
    }
}