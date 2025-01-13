using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Text.Json;
using System.Net.Http.Headers;
using RoadmapDesigner.Server.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// ������������ ��������� �����������
builder.Services.AddControllers();

// �������� ������ ����������� �� ������������
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� DbContext
builder.Services.AddDbContext<RoadmapContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// ��������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��������� ��������� CORS
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

app.UseCors("AllowAll"); // ���������� CORS

// ���������� ����������� �����
app.UseDefaultFiles();
app.UseStaticFiles();

// ������������ HTTP ��������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // ������� ��������������
app.UseAuthorization();  // ����� �����������
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
