using AutoFixture;
using EntityFrameworkCore.AutoFixture.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.TranslationTests.Sqlite;

/// <summary>
/// Класс конфигурации теста.
/// </summary>
public class Startup
{
    /// <summary>
    /// Конфигурация сервисов.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Fixture>();
        
        var fixture = new Fixture();
        fixture.Customize(new SqliteCustomization());
        services.AddScoped<DataContext>(_ => fixture.Create<DataContext>());
    }
}