var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Mood/Error"); // 或自定义错误页
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 🟢 设定默认控制器为 Mood，默认方法为 index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mood}/{action=Index}/{id?}");

app.Run();