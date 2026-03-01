using CleanArchitecture.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddGraphServices(builder => builder.AddDataLoaders());
builder.Services.AddWebServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    if (Environment.GetEnvironmentVariable("DRY_RUN") is null)
    {
        await app.InitialiseDatabaseAsync();
    }
}

app.UseRouting();

app.UseRequestLocalization(options =>
{
    options.SetDefaultCulture("en")
        .AddSupportedCultures("en")
        .AddSupportedUICultures("en");
});

app.UseAuthorization();

app.MapGraphQL();

return await app.RunWithGraphQLCommandsAsync(args);
