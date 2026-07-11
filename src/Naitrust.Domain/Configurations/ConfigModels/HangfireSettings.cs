namespace Naitrust.Domain.Configurations.ConfigModels;

public class HangfireSettings
{
    public bool DashboardEnabled { get; set; }
    public string? DashboardPath { get; set; }
    public string? ConnectionString { get; set; }
}
