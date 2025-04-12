using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using StoreVisitTracking.Application;
using StoreVisitTracking.Infrastructure;
using StoreVisitTracking.Application.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreVisitTracking API", Version = "v1" });

    // Add JWT Bearer authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApplicationServices();

// Configure DbContext with retry policy
builder.Services.AddDbContext<StoreVisitTrackingDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0)),
        mysqlOptions =>
        {
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 2,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null);
            mysqlOptions.MigrationsAssembly(typeof(StoreVisitTrackingDbContext).Assembly.FullName);
        });
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddMySql(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "mysql",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "mysql" })
    .AddRedis(
        redisConnectionString: builder.Configuration["Redis:Configuration"],
        name: "redis",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "cache", "redis" });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreVisitTracking API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                exception = e.Value.Exception?.Message,
                duration = e.Value.Duration
            }),
            totalDuration = report.TotalDuration
        });
    }
});

app.Run();