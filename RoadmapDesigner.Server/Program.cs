using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Text.Json;
using System.Net.Http.Headers;
using RoadmapDesigner.Server.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем остальные зависимости
builder.Services.AddControllers();

// Получаем строку подключения из конфигурации
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Настройка DbContext
builder.Services.AddDbContext<RoadmapContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Настройка Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавляем поддержку CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("AllowAll"); // Используем CORS

// Используем статические файлы
app.UseDefaultFiles();
app.UseStaticFiles();

// Конфигурация HTTP запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Сначала аутентификация
app.UseAuthorization();  // Затем авторизация
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
