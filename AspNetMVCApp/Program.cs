using MoodMusicProject; // 替换为你的 AppDbContext 命名空间
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ 配置 MySQL 数据库连接（从 appsettings 或环境变量）
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? Environment.GetEnvironmentVariable("MYSQLCONNSTR_DefaultConnection") 
                      ?? "server=gondola.proxy.rlwy.net;port=26046;database=railway;user=root;password=lNekNpVQlvjcVIiXWQSWMagPlRkIJlSr;";

// 2️⃣ 注册 EF Core 服务
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 3️⃣ 自动迁移：部署到 Render 后自动创建表结构
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// 4️⃣ 配置 HTTP 管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Mood/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mood}/{action=Index}/{id?}");

app.Run();