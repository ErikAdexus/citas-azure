using Citas.Api.Services;
using Citas.Api.Repositories;
using Citas.Api.Data;                 // para AppDbContext
using Microsoft.EntityFrameworkCore;  // para UseSqlServer
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var vaultUri = builder.Configuration["KeyVault:Uri"];
if (!string.IsNullOrWhiteSpace(vaultUri))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(vaultUri),
        new DefaultAzureCredential());
}

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Registro de dependencias (Inyección de Dependencias)
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();


// 1. CORS: los orígenes permitidos se leen de la configuración (Cors:AllowedOrigins)
//    - En Azure: variable de entorno Cors__AllowedOrigins__0 = URL del front
//    - En local: appsettings.Development.json (ver abajo)
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
                        // .WithOrigins("http://localhost:4200")   // Versión local (fija): ahora va en appsettings.Development.json
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RealConnection"),
        sql => sql.EnableRetryOnFailure()));   // útil con la base gratuita que se pausa


if (!string.IsNullOrWhiteSpace(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor();

    // Agrega esta línea para que exista el TelemetryClient en el contenedor de dependencias:
    builder.Services.AddApplicationInsightsTelemetry();
}

var app = builder.Build();

app.Logger.LogInformation("Application Insights configurado: {Configured}",
    !string.IsNullOrWhiteSpace(app.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 2. Usar la política de CORS antes de los controladores
app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapGet("/", () => "Citas API funcionando");
app.Run();

