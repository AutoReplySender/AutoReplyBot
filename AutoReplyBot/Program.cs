using System.Net;
using System.Text;
using System.Text.Json;
using Band;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoReplyBot;

public class Program
{
    public record Config(string Email, string Password, string ChromeDriverDir, string ChromePath,
        string Proxy, int MaxTriggerTimesBySinglePost, string? EndPoint, string ConnectionString);

    public static async Task<List<Rule>> LoadRules()
    {
        var content = await File.ReadAllTextAsync("configs/rules.yaml");
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        var rules = deserializer.Deserialize<List<Rule>>(content);
        rules
            .AsParallel()
            .SelectMany(rule => rule.Replies)
            .Where(reply => reply.ReplyType == ReplyType.CSharpScript)
            .ForAll(reply => reply.Script = Script.CreateDelegate<string>(reply.Data, typeof(Global)));
        return rules;
    }

    public static async Task Main(string[] args)
    {
        #region Set up console

        // That supports both Windows and Linux at any locale
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        #endregion

        var config = JsonSerializer.Deserialize<Config>(await File.ReadAllBytesAsync("configs/config.json"))!;
        var (email, password, chromeDriverDir, chromePath, proxy, maxTriggerTimesBySinglePost, endPoint,
            connectionString) = config;
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("configs/config.json")
            .Build();
        services.AddLogging(builder =>
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // make EF Core logging less verbose
                .CreateLogger();
            builder.AddSerilog();
        });
        services.AddScoped(sp =>
        {
            string cookies;
            if (args.Contains("--login"))
            {
                var helper = sp.GetRequiredService<CookieHelper>();
                cookies = helper.GetCookiesFromBrowser(email, password, chromeDriverDir, chromePath,
                    args.Contains("--headless"));
                File.WriteAllText("configs/saved.cookies", cookies);
            }
            else
            {
                // remove \n, \r 
                cookies = File.ReadAllText("configs/saved.cookies").Trim();
            }

            return new BandClientOptions
            {
                CookieContainer = BandClient.CreateCookieContainer(cookies),
                Suffix =
                    "I am a bot, and this action was performed automatically. Please contact 兔友小e if you have any questions or concerns.",
                EndPoint = endPoint
            };
        });
        services.AddScoped<IWebProxy>(_ => new WebProxy(proxy));
        services.AddScoped<HttpPing>();
        services.AddScoped(sp =>
            new Matcher(LoadRules().Result, maxTriggerTimesBySinglePost, sp.GetRequiredService<ILogger<Matcher>>()));
        // services.AddScoped<BandClient, MockBandClient>();
        services.AddScoped<BandClient>();
        services.AddScoped<CookieHelper>();
        services.AddScoped<Producer>();
        services.AddScoped<Consumer>();
        var options = new DbContextOptionsBuilder<AutoReplyContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;
        var factory = new PooledDbContextFactory<AutoReplyContext>(options);
        services.AddTransient(_ => factory.CreateDbContext());
        var sp = services.BuildServiceProvider();
        await sp.GetRequiredService<Producer>().Produce();
    }
}