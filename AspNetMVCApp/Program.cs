var builder = WebApplication.CreateBuilder(args);

// 添加 MVC 控制器支持
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 配置 HTTP 请求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Mood/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 默认路由设定：Mood 控制器 + Index 方法
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mood}/{action=Index}/{id?}");

app.Run();