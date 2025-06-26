using ApplicationEnglishLearning.Validate;
using DataBaseServices;
using ApplicationEnglishLearning.Extensions;
using Infrastructure.Authentication;

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5000); 
    options.ListenAnyIP(7041, listenOptions => 
    {
        listenOptions.UseHttps(); 
    });
});


// Add services to the container.

builder.Services.AddControllers();
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
builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddAuthorization();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();


await using var scope = app.Services.CreateAsyncScope();
var init = scope.ServiceProvider.GetRequiredService<Initialization>();
if (!await init.InitializeAsync())
    throw new Exception("База данный не инициализировалась!");
app.Run("https://0.0.0.0:7041");
