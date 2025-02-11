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
using System.Security.Claims;
using RoadmapDesigner.Server.Models.DTO;
using RoadmapDesigner.Server.Decorators;


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


    builder.Services.AddSingleton(typeof(ILogger<>), typeof(TimeStampLoggerDecorator<>)); // Стандартный логгер

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
    .AddOAuth("GitHub", options => // Конфигурация OAuth для GitHub
    {
        options.ClientId = builder.Configuration["GitHub:ClientId"]; // Получаем client_id из конфигурации
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"]; // Получаем client_secret из конфигурации
        options.CallbackPath = new PathString("/signin-github"); // Должен совпадать с redirect_uri

      options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
      options.TokenEndpoint = "https://github.com/login/oauth/access_token";
      options.UserInformationEndpoint = "https://api.github.com/user";

      options.SaveTokens = true; // Сохраняем токены в cookie

      options.ClaimActions.MapJsonKey("id", "id");
      options.ClaimActions.MapJsonKey("login", "login");
      options.ClaimActions.MapJsonKey("name", "name");
      options.ClaimActions.MapJsonKey("email", "email");


      options.Events = new OAuthEvents
      {
          OnCreatingTicket = async context =>
          {
              var loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
              var logger = loggerFactory.CreateLogger("OAuthEvents");
              try
              {
                  var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
                  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                  request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                  var response = await context.Backchannel.SendAsync(request).ConfigureAwait(false);
                  response.EnsureSuccessStatusCode();

                  var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                  var userLoginClaim = user.RootElement.GetProperty("login").GetString();
                  var userEmailClaim = user.RootElement.GetProperty("email").GetString();
                  var userFullNameClaim = user.RootElement.TryGetProperty("name", out var nameElement) ? nameElement.GetString() : null;

                  //получите из базы данных роль пользователя по login
                  var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                  var userDto = await userRepository.GetUserByLoginAsync(userLoginClaim); ;

                  if (userDto == null)
                  {
                      logger.LogInformation($"Пользователь с login: {userLoginClaim} не найден в базе данных, создаем пользователя.");

                      var newUser = new User
                      {
                          LastName = "default",
                          FirstName = "default",
                          MiddleName = "default",
                          Login = userLoginClaim,
                          Email = "default@mail.ru",
                          RoleId = 1
                      };
                      await userRepository.AddUserAsync(newUser);
                      userDto = new UserDTO
                      {
                          Login = newUser.Login,
                          Email = newUser.Email,
                          FirstName = newUser.FirstName,
                          LastName = newUser.LastName,
                          Role = "user"
                      };

                      logger.LogInformation($"Пользователь с login: {userLoginClaim} успешно добавлен в базу данных.");

                  }
                  // Добавляем роль пользователя в клеймы
                  context.Identity.AddClaim(new Claim(ClaimTypes.Role, userDto.Role));
                  context.RunClaimActions(user.RootElement);
              }
              catch (Exception ex)
              {
                  logger.LogError(ex, "Ошибка при обращении к API GitHub или базе данных.");
                  context.Fail("Ошибка авторизации");
                  return;
              }
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