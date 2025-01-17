using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.Entities;
using RoadmapDesigner.Server.Repositories;
using RoadmapDesigner.Server.Services;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


var builder = WebApplication.CreateBuilder(args);

try
{
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
    var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<RoadmapContext>(options =>
        options.UseNpgsql(defaultConnectionString));

    // Конфигурация аутентификации
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "TPU";
    })
    .AddCookie()
    .AddOAuth("TPU", options =>
    {
        options.ClientId = builder.Configuration["TPU:ClientId"]; // Получаем client_id из конфигурации
        options.ClientSecret = builder.Configuration["TPU:ClientSecret"]; // Получаем client_secret из конфигурации
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
                var apiKey = builder.Configuration["TPU:ApiKey"]; // Загрузка apiKey из конфигурации
                var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                request.Headers.Add("apiKey", apiKey); // Добавление apiKey в заголовок запроса
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await context.Backchannel.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                context.RunClaimActions(user.RootElement);
            }
        };
    });

    // Добавление CORS, Swagger и других сервисов
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Настройка CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

var app = builder.Build();

try
{
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
    app.UseAuthentication();
    app.UseAuthorization();

    // Маппинг контроллеров
    app.MapControllers();
    app.MapFallbackToFile("/index.html");

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}