using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Models.Entity;

var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ������������
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� ������ ���� ������
builder.Services.AddDbContext<RoadmapDesignerContext>(options =>
    options.UseNpgsql(connectionString));

// ��������� ������������
builder.Services.AddControllers();

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
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
