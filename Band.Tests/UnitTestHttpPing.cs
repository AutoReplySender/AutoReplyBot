using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Band.Tests;

public class UnitTestHttpPing
{
    private readonly ILogger<UnitTestHttpPing> _logger;
    private readonly ServiceProvider _serviceProvider;

    public UnitTestHttpPing(ITestOutputHelper testOutputHelper)
    {
        _serviceProvider = Common.GetServiceCollection(testOutputHelper).BuildServiceProvider();
        _logger = _serviceProvider.GetRequiredService<ILogger<UnitTestHttpPing>>();
    }

    [Fact]
    public async Task TestHttpPing()
    {
        var httpPing = _serviceProvider.GetRequiredService<HttpPing>();
        await httpPing.Ping("https://www.google.com");
    }

    [Fact]
    public async Task TestGetFastest()
    {
        var httpPing = _serviceProvider.GetRequiredService<HttpPing>();
        await httpPing.GetFastest(BandClient.Endpoints);
    }
}