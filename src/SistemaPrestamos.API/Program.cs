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
builder.Services.AddScoped<IImportacionService, ImportacionService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IPeriodoInteresService, PeriodoInteresService>();
builder.Services.AddScoped<IMorosidadService, MorosidadService>();
builder.Services.AddScoped<IGaranteService, GaranteService>();
builder.Services.AddScoped<IDispositivoService, DispositivoService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IMensajeWhatsappRepository, MensajeWhatsappRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IFondoRepository, FondoRepository>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();builder.Services.AddHttpClient<IWhatsappService, WhatsappService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
builder.Services.AddScoped<IReglaNotificacionService, ReglaNotificacionService>();
builder.Services.AddScoped<IProgramadorService, ProgramadorService>();
builder.Services.AddHostedService<ProgramadorJob>();
builder.Services.AddHostedService<MorosidadJob>();

// Repositories
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<IPeriodoInteresRepository, PeriodoInteresRepository>();
builder.Services.AddScoped<IMorosidadRepository, MorosidadRepository>();
builder.Services.AddScoped<IGaranteRepository, GaranteRepository>();
builder.Services.AddScoped<IDispositivoRepository, DispositivoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddScoped<IReglaNotificacionRepository, ReglaNotificacionRepository>();
builder.Services.AddScoped<IEnvioNotificacionRepository, EnvioNotificacionRepository>();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Puerto de la nube (Render inyecta PORT); local usa launchSettings.
var puertoNube = Environment.GetEnvironmentVariable("PORT");

if (!string.IsNullOrWhiteSpace(puertoNube))
{
    app.Urls.Clear();
    app.Urls.Add($"http://*:{puertoNube}");
}

// Aplica migraciones pendientes al arrancar (nube y local).
using (var alcance = app.Services.CreateScope())
{
    var baseDatos = alcance.ServiceProvider
        .GetRequiredService<PrestamosDbContext>();

    baseDatos.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Sin redirección HTTPS: Render termina TLS en su proxy y la
// API solo recibe HTTP interno. La URL pública igual es https.
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

// Ping liviano para cron-job.org / Render (2 bytes, sin auth ni DB).
// Evita "salida demasiado grande" y sirve como keep-alive.
app.MapGet("/ping", () => Results.Text("OK")).AllowAnonymous();

// Ejecutor externo: cron-job.org lo llama cada 5 min aunque nadie
// abra la app. Si Render dormía, al despertar ejecuta lo pendiente
// dentro de la ventana de 15 min. Proteger con Cron:Secret.
app.MapPost("/api/cron/ejecutar-pendientes", async (
    HttpContext ctx,
    IProgramadorService programador,
    IConfiguration cfg) =>
{
    var secreto = cfg["Cron:Secret"];

    if (!string.IsNullOrWhiteSpace(secreto))
    {
        var enviado = ctx.Request.Headers["X-Cron-Secret"].ToString();

        if (enviado != secreto)
        {
            return Results.Unauthorized();
        }
    }

    var ahoraUtc = DateTime.UtcNow;
    var enviados = await programador.EjecutarPendientesAsync(ahoraUtc);
    var lima = ProgramadorService.AhoraLima(ahoraUtc);

    return Results.Ok(new
    {
        enviados,
        horaLima = lima.ToString("HH:mm:ss"),
        ultimoTickUtc = SistemaPrestamos.API.Jobs.ProgramadorJob.UltimoTickUtc?.ToString("HH:mm:ss")
    });
}).AllowAnonymous();

app.Run();