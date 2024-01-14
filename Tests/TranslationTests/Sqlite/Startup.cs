using AutoFixture;
using EntityFrameworkCore.AutoFixture.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.TranslationTests.Sqlite;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Fixture>();
        
        var fixture = new Fixture();
        fixture.Customize(new SqliteCustomization());
        services.AddScoped<DataContext>(_ => fixture.Create<DataContext>());
    }
}