using Hangfire;
using Naitrust.Api.Configuration;
using Naitrust.Api.Middleware;
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
