namespace Straddle.Payments.Infrastructure;

public class InfrastructureOptions
{
    public string? DatabaseConnectionString { get; set; }
    public string? ServiceBusConnectionString { get; set; }
}