using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Band.Tests;

public class UnitTestBandClient
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ILogger<UnitTestBandClient> _logger;

    public UnitTestBandClient(ITestOutputHelper testOutputHelper)
    {
        var services = Common.GetServiceCollection(testOutputHelper);
        services.AddScoped<BandClient>();
        _serviceProvider = services.BuildServiceProvider();
        _logger = _serviceProvider.GetRequiredService<ILogger<UnitTestBandClient>>();
    }

    [Fact]
    public void TestInit()
    {
        _serviceProvider.GetRequiredService<BandClient>();
    }

    [Fact]
    public async Task TestGetFeed()
    {
        var client = _serviceProvider.GetRequiredService<BandClient>();
        var feed = await client.GetFeedAsync();
        _logger.LogInformation("{Feed}", feed.PrettyFormat());
        Assert.True(feed.Items.Count > 0);
    }
}