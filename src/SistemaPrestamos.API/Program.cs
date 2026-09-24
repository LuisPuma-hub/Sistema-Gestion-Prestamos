using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.API.Jobs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Infrastructure.Data;
using SistemaPrestamos.Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "La configuración Jwt:Key no está definida.");
}

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Entity Framework Core + PostgreSQL
builder.Services.AddDbContext<PrestamosDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Application Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPrestamoService, PrestamoService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IPeriodoInteresService, PeriodoInteresService>();
builder.Services.AddScoped<IMorosidadService, MorosidadService>();
builder.Services.AddScoped<IGaranteService, GaranteService>();
builder.Services.AddScoped<IDispositivoService, DispositivoService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IMensajeWhatsappRepository, MensajeWhatsappRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddHttpClient<IWhatsappService, WhatsappService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReglaNotificacionService, ReglaNotificacionService>();
builder.Services.AddScoped<IProgramadorService, ProgramadorService>();
builder.Services.AddHostedService<ProgramadorJob>();

// Repositories
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<IPeriodoInteresRepository, PeriodoInteresRepository>();
builder.Services.AddScoped<IMorosidadRepository, MorosidadRepository>();
builder.Services.AddScoped<IGaranteRepository, GaranteRepository>();
builder.Services.AddScoped<IDispositivoRepository, DispositivoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IReglaNotificacionRepository, ReglaNotificacionRepository>();
builder.Services.AddScoped<IEnvioNotificacionRepository, EnvioNotificacionRepository>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// HTTPS temporalmente desactivado para pruebas locales
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();