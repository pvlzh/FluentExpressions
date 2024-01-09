using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Fixture>();
    }
}