namespace CleanArchitecture.Infrastructure.Data;

public class DbContextFactoryOptions
{
    public string? ConnectionString { get; set; }

    public string? Provider { get; set; }
}
