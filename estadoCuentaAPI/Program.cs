using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using estadoCuentaAPI.Models;
using estadoCuentaAPI.Services;
using estadoCuentaAPI.Validators;
using estadoCuentaAPI.Interfaces;
using estadoCuentaAPI.HealthChecks;
using Newtonsoft.Json;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Estado de cuenta API",
        Version = "v1",
        Description = "Documentación de la API con Swagger",
        Contact = new OpenApiContact
        {
            Name = "Jose Ricardo Martinez Ruano",
            Email = "03.jose@gmail.com",
            Url = new Uri("https://tuweb.com")
        }
    });
});

// Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar el contexto de base de datos
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Agregar Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<CustomHealthCheck>("self", tags: new[] { "api" })
    .AddSqlServer(connectionString, healthQuery: "SELECT 1;", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "sql", "sqlserver" });

// Registros de FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<TarjetaCreditoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ClienteValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MovimientoTarjetaValidator>();

// Servicios de la API
builder.Services.AddScoped<IMovimientoService, MovimientoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ITarjetaCreditoService, TarjetaCreditoService>();

// Configuración de CORS
var frontEndUrl = builder.Configuration["frontEndUrl"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
        policy.WithOrigins(frontEndUrl)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configuración del middleware de Swagger solo en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Estado de cuenta API v1");
        options.RoutePrefix = "swagger";
    });
}

// Configuración de CORS
app.UseCors("AllowMVC");

// Redirección a HTTPS
app.UseHttpsRedirection();

app.UseRouting();

// Configuración de Health Checks
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() }),
                duration = report.TotalDuration.TotalSeconds
            });
            await context.Response.WriteAsync(result);
        }
    });
});

// Configuración de autorización y enrutamiento
app.UseAuthorization();
app.MapControllers();

app.Run();
