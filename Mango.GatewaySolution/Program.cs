using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = builder.Configuration.GetValue<string>("AuthenticationAuthority");
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false
    };
});

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Crear una instancia del ConfigurationBuilder para combinar las configuraciones
var configuration = new ConfigurationBuilder().Build();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();