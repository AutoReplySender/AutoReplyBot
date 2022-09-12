using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace Band.Tests;

public class Common
{
    public static ServiceCollection GetServiceCollection(ITestOutputHelper testOutputHelper)
    {
        var services = new ServiceCollection();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.TestOutput(testOutputHelper)
            .CreateLogger();
        services.AddLogging(builder => builder.AddSerilog());
        services.AddScoped(_ => new BandClientOptions
        {
            CookieContainer = BandClient.CreateCookieContainer(File.ReadAllText("saved.cookies")),
            EndPoint = "https://api-kr.band.us"
        });
        services.AddScoped<IWebProxy>(_ => new WebProxy("http://127.0.0.1:8888"));
        services.AddScoped<HttpPing>();
        return services;
    }
}