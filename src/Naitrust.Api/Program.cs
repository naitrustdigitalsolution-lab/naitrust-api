using Hangfire;
using Naitrust.Api.Configuration;
using Naitrust.Api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
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

// Register recurring background jobs
HangfireJobRegistration.RegisterAll(app.Services);

app.Run();

// Make the Program class accessible to the integration test project
public partial class Program { }
