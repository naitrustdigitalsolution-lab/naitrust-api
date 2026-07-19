using Hangfire;
using Microsoft.EntityFrameworkCore;
using Naitrust.Api.Configuration;
using Naitrust.Api.Middleware;
using Naitrust.Infrastructure.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Railway sets PORT env var
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


// Serilog
builder.AddSerilogLogging();

// All services (Infrastructure + Application + API-level)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAllServices(builder.Configuration);

var app = builder.Build();

// Apply pending migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NaitrustDbContext>();
    var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
    {
        app.Logger.LogInformation("Applying {Count} pending migration(s)...", pendingMigrations.Count());
        await db.Database.MigrateAsync();
    }
}

// Middleware pipeline
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("NaitrustCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapSignalRHubs();

// Hangfire dashboard
var hangfireSettings = builder.Configuration.GetSection("Hangfire");
if (hangfireSettings.GetValue<bool>("DashboardEnabled"))
{
    app.UseHangfireDashboard(hangfireSettings.GetValue<string>("DashboardPath") ?? "/hangfire");
}

// Register recurring background jobs (non-fatal if DB not ready)
try
{
    HangfireJobRegistration.RegisterAll(app.Services);
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to register Hangfire jobs. Ensure DATABASE_URL is configured.");
}

app.Run();

// Make the Program class accessible to the integration test project
public partial class Program { }
