using AutoFixture;
using EntityFrameworkCore.AutoFixture.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.TranslationTests.Sqlite;

/// <summary>
/// DI test configuration.
/// </summary>
public class Startup
{
    /// <summary>
    /// Configure services.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        var fixture = new Fixture();
        services.AddSingleton(fixture);
        
        fixture.Customize(new SqliteCustomization());
        services.AddScoped<DataContext>(_ => fixture.Create<DataContext>());
    }
}