using ApplicationEnglishLearning.Services;
using ApplicationEnglishLearning.Validate;
using DataBaseServices;
using System.Diagnostics;
using System.Text;
using ApplicationEnglishLearning.Extensions;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP
    options.ListenAnyIP(7041, listenOptions => // HTTPS
    {
        listenOptions.UseHttps(); // Используйте сертификат по умолчанию
    });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ValidateWordFilter>();
builder.Services.AddTransient<IAuthService, PasswordHash>();
builder.Services.AddTransient<ILogin, UserService>();


builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddTransient<IGenerateToken, Jwt>();



builder.Services.Configure<BdSettings>(
    builder.Configuration.GetSection("DataBase"));
builder.Services.AddDataBaseServices();
builder.Services.AddSingleton<ITranslateDictionary<string, string>, TranslateCollection>();

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Важно: сначала аутентификация, потом авторизация
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();


await using var scope = app.Services.CreateAsyncScope();
var init = scope.ServiceProvider.GetRequiredService<Initialization>();
if (!await init.InitializeAsync())
    throw new Exception("База данный не инициализировалась!");
app.Run("https://0.0.0.0:7041");
