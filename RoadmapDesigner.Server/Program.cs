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
    // ����������� ��������
    builder.Services.AddScoped<IDirectionTrainingRepository, DirectionTrainingRepository>();
    builder.Services.AddScoped<IDirectionTrainingService, DirectionTrainingService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IDisciplineRepository, DisciplineRepository>();
    builder.Services.AddScoped<IDisciplineService, DisciplineService>();
    builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
    builder.Services.AddScoped<ISpecializationService, SpecializationService>();


    builder.Services.AddSingleton(typeof(ILogger<>), typeof(TimeStampLoggerDecorator<>)); // ����������� ������

    // ���������� DbContext
    var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<RoadmapContext>(options =>
        options.UseNpgsql(defaultConnectionString));

    // ������������ ��������������
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "TPU";
    })
    .AddCookie()
    .AddOAuth("GitHub", options => // ������������ OAuth ��� GitHub
    {
        options.ClientId = builder.Configuration["GitHub:ClientId"]; // �������� client_id �� ������������
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"]; // �������� client_secret �� ������������
        options.CallbackPath = new PathString("/signin-github"); // ������ ��������� � redirect_uri

      options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
      options.TokenEndpoint = "https://github.com/login/oauth/access_token";
      options.UserInformationEndpoint = "https://api.github.com/user";

      options.SaveTokens = true; // ��������� ������ � cookie

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

                  //�������� �� ���� ������ ���� ������������ �� login
                  var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                  var userDto = await userRepository.GetUserByLoginAsync(userLoginClaim); ;

                  if (userDto == null)
                  {
                      logger.LogInformation($"������������ � login: {userLoginClaim} �� ������ � ���� ������, ������� ������������.");

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

                      logger.LogInformation($"������������ � login: {userLoginClaim} ������� �������� � ���� ������.");

                  }
                  // ��������� ���� ������������ � ������
                  context.Identity.AddClaim(new Claim(ClaimTypes.Role, userDto.Role));
                  context.RunClaimActions(user.RootElement);
              }
              catch (Exception ex)
              {
                  logger.LogError(ex, "������ ��� ��������� � API GitHub ��� ���� ������.");
                  context.Fail("������ �����������");
                  return;
              }
          }
      };
  });


    // ���������� CORS, Swagger � ������ ��������
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // ��������� CORS
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
    app.UseCors("AllowAll"); // ���������� CORS

    // ������������� ����������� ������
    app.UseDefaultFiles();
    app.UseStaticFiles();

    // ��������� Swagger � ������ ����������
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // ��������� �������������� � �����������
    app.UseAuthentication();
    app.UseAuthorization();

    // ������� ������������
    app.MapControllers();
    app.MapFallbackToFile("/index.html");

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}