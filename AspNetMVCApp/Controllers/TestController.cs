using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace MoodMusicProject.Controllers
{
    public class TestController : Controller
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Db()
        {
            try
            {
                var connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    return Content("✅ Successfully connected to MySQL!");
                }
            }
            catch (Exception ex)
            {
                return Content("❌ Failed to connect to MySQL: " + ex.Message);
            }
        }
    }
}