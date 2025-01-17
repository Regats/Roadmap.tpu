using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.Entities;
using RoadmapDesigner.Server.Repositories;
using RoadmapDesigner.Server.Services;
using System.Net.Http.Headers;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddScoped<IDirectionTrainingRepository, DirectionTrainingRepository>();
builder.Services.AddScoped<IDirectionTrainingService, DirectionTrainingService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDisciplineRepository, DisciplineRepository>();
builder.Services.AddScoped<IDisciplineService, DisciplineService>();
builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();

// Добавление DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RoadmapContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "TPU";
})
.AddCookie()
.AddOAuth("TPU", options =>
{
    options.ClientId = "Ваш client_id"; // Полученный client_id из TPU
    options.ClientSecret = "Ваш client_secret"; // Полученный client_secret из TPU
    options.CallbackPath = new PathString("/signin-tpu"); // Должен совпадать с redirect_uri

    options.AuthorizationEndpoint = "https://oauth.tpu.ru/authorize";
    options.TokenEndpoint = "https://oauth.tpu.ru/access_token";
    options.UserInformationEndpoint = "https://api.tpu.ru/v2/auth/user";

    options.SaveTokens = true; // Сохраняем токены в cookie

    options.ClaimActions.MapJsonKey("id", "user_id");
    options.ClaimActions.MapJsonKey("email", "email");
    options.ClaimActions.MapJsonKey("name", "lichnost.imya");
    options.ClaimActions.MapJsonKey("surname", "lichnost.familiya");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await context.Backchannel.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            context.RunClaimActions(user.RootElement);
        }
    };
});

// Добавление CORS, Swagger и других сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll"); // Используем CORS

// Использование статических файлов
app.UseDefaultFiles();
app.UseStaticFiles();

// Настройка Swagger в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Добавляем аутентификацию и авторизацию
app.UseAuthentication(); // Сначала аутентификация
app.UseAuthorization();  // Затем авторизация

// Маппинг контроллеров
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
