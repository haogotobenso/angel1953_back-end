using Microsoft.EntityFrameworkCore;
using angel1953_backend.Models;

var builder = WebApplication.CreateBuilder(args);

// 添加服務，包括 DbContext 並配置 SQL Server 連接字串
builder.Services.AddDbContext<Db>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// 配置 HTTP 請求管道
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
