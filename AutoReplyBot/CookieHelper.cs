using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace AutoReplyBot;

public class CookieHelper
{
    private readonly ILogger<CookieHelper> _logger;

    public CookieHelper(ILogger<CookieHelper> logger)
    {
        _logger = logger;
    }

    public string GetCookiesFromBrowser(string email, string password, string chromeDriverDir, string chromePath,
        bool headless)
    {
        var options = new ChromeOptions { BinaryLocation = chromePath };
        if (headless)
        {
            options.AddArguments("headless");
        }

        var driver = new ChromeDriver(chromeDriverDir, options);
        driver.Navigate().GoToUrl("https://auth.band.us/email_login?keep_login=true");
        driver.FindElement(By.Id("input_email")).SendKeys(email);
        driver.FindElement(By.CssSelector("#email_login_form > button")).Click();
        driver.FindElement(By.Id("pw")).SendKeys(password);
        driver.FindElement(By.CssSelector("#email_password_login_form > button")).Click();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        wait.Until(d => d.Url.StartsWith("https://band.us"));
        driver.Navigate().GoToUrl("https://auth.band.us");
        var cookies = (string)driver.ExecuteScript("return document.cookie");
        _logger.LogDebug("Got cookies from browser: {Cookies}", cookies);
        driver.Navigate().GoToUrl("https://band.us");
        return cookies;
    }
}