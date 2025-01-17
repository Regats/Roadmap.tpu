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

// ����������� ��������
builder.Services.AddScoped<IDirectionTrainingRepository, DirectionTrainingRepository>();
builder.Services.AddScoped<IDirectionTrainingService, DirectionTrainingService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDisciplineRepository, DisciplineRepository>();
builder.Services.AddScoped<IDisciplineService, DisciplineService>();
builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();

// ���������� DbContext
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
    options.ClientId = "��� client_id"; // ���������� client_id �� TPU
    options.ClientSecret = "��� client_secret"; // ���������� client_secret �� TPU
    options.CallbackPath = new PathString("/signin-tpu"); // ������ ��������� � redirect_uri

    options.AuthorizationEndpoint = "https://oauth.tpu.ru/authorize";
    options.TokenEndpoint = "https://oauth.tpu.ru/access_token";
    options.UserInformationEndpoint = "https://api.tpu.ru/v2/auth/user";

    options.SaveTokens = true; // ��������� ������ � cookie

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

// ���������� CORS, Swagger � ������ ��������
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
app.UseAuthentication(); // ������� ��������������
app.UseAuthorization();  // ����� �����������

// ������� ������������
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
