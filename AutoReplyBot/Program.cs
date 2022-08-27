using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Band;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AutoReplyBot;

class Program
{
    public record Config(string Email, string Password, string ChromeDriverDir, string ChromePath, int Consumers,
        string Proxy);

    public static async Task<List<Rule>> LoadRules()
    {
        await using var db = new AutoReplyContext();
        return await db.Rules.ToListAsync();
    }

    public static BandClient InitBandClient(string cookies, string proxy)
    {
        return new BandClient(cookies, proxy)
        {
            Suffix =
                "I am a bot, and this action was performed automatically. Please contact nobody if you have any questions or concerns."
        };
    }

    public static string GetCookiesFromBrowser(string email, string password, string chromeDriverDir, string chromePath)
    {
        var options = new ChromeOptions {BinaryLocation = chromePath};
        var driver = new ChromeDriver(chromeDriverDir, options);
        driver.Navigate().GoToUrl("https://auth.band.us/email_login?keep_login=true");
        driver.FindElement(By.Id("input_email")).SendKeys(email);
        driver.FindElement(By.CssSelector("#email_login_form > button")).Click();
        driver.FindElement(By.Id("pw")).SendKeys(password);
        driver.FindElement(By.CssSelector("#email_password_login_form > button")).Click();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        wait.Until(d => d.Url.StartsWith("https://band.us"));
        driver.Navigate().GoToUrl("https://auth.band.us");
        var cookies = (string) driver.ExecuteScript("return document.cookie");
#if DEBUG
        Console.WriteLine(cookies);
#endif
        driver.Navigate().GoToUrl("https://band.us");
        return cookies;
    }

    public static async Task Main(string[] args)
    {
        #region Set up console

        // That supports both Windows and Linux at any locale
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        #endregion

        var config = JsonSerializer.Deserialize<Config>(await File.ReadAllBytesAsync("config.json"))!;
        var (email, password, chromeDriverDir, chromePath, consumers, proxy) = config;
        var matcher = new Matcher(await LoadRules());
        var cookies = GetCookiesFromBrowser(email, password, chromeDriverDir, chromePath);
        var client = InitBandClient(cookies, proxy);
        var channel = new BlockingCollection<ChannelData>();
        for (var i = 0; i < consumers; i++)
        {
            _ = Task.Run(new Consumer(channel, client, matcher).Consume);
        }

        Task.Run(new Producer(channel, client).Produce).Wait();
    }
}