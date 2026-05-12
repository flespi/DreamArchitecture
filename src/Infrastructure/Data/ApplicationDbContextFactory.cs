using System.CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CleanArchitecture.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var options = ParseArgs(args);

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        switch (options.Provider?.ToLower())
        {
            case null:
            case "sqlserver":
                builder.UseSqlServer(options.ConnectionString);
                break;
            default:
                throw new ApplicationException("Unknown database provider.");
        }

        return new ApplicationDbContext(builder.Options);
    }

    public static DbContextFactoryOptions ParseArgs(string[] args)
    {
        var connectionOption = new Option<string?>("--connection-string", "-c")
        {
            Description = "Connection String"
        };

        var providerOption = new Option<string?>("--provider", "-p")
        {
            Description = "Provider"
        };

        var root = new RootCommand
        {
            connectionOption,
            providerOption,
        };

        var result = root.Parse(args);

        return new DbContextFactoryOptions
        {
            ConnectionString = result.GetValue(connectionOption),
            Provider = result.GetValue(providerOption),
        };
    }
}
