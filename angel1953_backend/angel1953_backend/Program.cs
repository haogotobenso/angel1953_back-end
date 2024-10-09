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
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5280")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
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
builder.Services.AddScoped<MemberServices>();
builder.Services.AddScoped<FrontService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<FrontRepository>();



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
// 配置 HTTP 請求管道
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
