using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using angel1953_backend.Models;
using angel1953_backend.Services;
using angel1953_backend.Repository;
using angel1953_backend.Controllers;

var builder = WebApplication.CreateBuilder(args);

// 添加服務，包括 DbContext 並配置 SQL Server 連接字串
builder.Services.AddDbContext<angel1953Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var JWT = builder.Configuration.GetSection("JWT");
var KEY = JWT["KEY"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"])),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Teacher", policy =>
    {
        policy.RequireRole("Teacher");
    });
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<MemberServices>();
builder.Services.AddScoped<FrontService>();
builder.Services.AddScoped<BackService>();
builder.Services.AddScoped<PythonService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<FrontRepository>();
builder.Services.AddScoped<BackRepository>();




var app = builder.Build();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();
// 配置 HTTP 請求管道
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
