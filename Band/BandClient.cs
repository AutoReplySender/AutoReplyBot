using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using static Band.Helper;

namespace Band;

public class BandClientOptions
{
    // CookieContainer can't be modified after BandClient initialized so disallow modifying here
    // Others can be changed, but why? Disallow it too since it requires more code to work.
    public required CookieContainer CookieContainer { get; init; }
    public string Suffix { get; init; } = "Sent from BAND Client Library";
    public string? EndPoint { get; init; }
}

public partial class BandClient
{
    protected readonly ILogger<BandClient> Logger;

    public static readonly string[] Endpoints =
    {
        "https://api.band.us",
        "https://api-de.band.us",
        "https://api-kr.band.us",
        "https://api-us.band.us",
        "https://api-usw.band.us",
        "https://api-sg.band.us",
    };

    public static CookieContainer CreateCookieContainer(string cookies)
    {
        var cookieContainer = new CookieContainer();
        var cookieList = ParseCookies(cookies);
        foreach (var cookie in cookieList)
        {
            cookie.Path = "/";
            cookie.Domain = ".band.us";
            cookieContainer.Add(cookie);
        }

        return cookieContainer;
    }

    private const string UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36";

    private readonly CookieContainer _cookieContainer;
    private readonly HttpClient _httpClient;

    private readonly string _suffix;

    // We will set SecretKey in RefreshAsync() so suppress the uninitialized warning
    private string _secretKey = null!;

    public BandClient(BandClientOptions options, HttpPing httpPing, ILogger<BandClient> logger, IWebProxy? proxy = null)
    {
        Logger = logger;
        _cookieContainer = options.CookieContainer;
        _suffix = options.Suffix;

        #region Initialize HttpClientHandler

        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = _cookieContainer,
            Proxy = proxy
        };

        #endregion

        #region Initialize HttpClient

        _httpClient = new HttpClient(httpClientHandler)
        {
            DefaultRequestHeaders =
            {
                { "user-agent", UserAgent },
                { "akey", "bbc59b0b5f7a1c6efe950f6236ccda35" },
                { "device-time-zone-id", "Europe/Paris" } // Rabbit House's time zone
            },
            BaseAddress = new Uri(options.EndPoint ?? httpPing.GetFastest(Endpoints).Result)
        };

        #endregion

        RefreshAsync().Wait();
    }

    public async Task<HttpResponseMessage> RefreshAsync()
    {
        var response = await _httpClient.GetAsync("https://auth.band.us/refresh");
        var secretKey = _cookieContainer.GetAllCookies().FirstOrDefault(c => c.Name == "secretKey")?.Value;
        if (string.IsNullOrEmpty(secretKey)) throw new AuthenticationException("Cookies expired.");

        _secretKey = secretKey.Trim('"');
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string uri, IDictionary<string, string> form)
    {
        form.Add("ts", (GetUnixTimeStamp() - Random.Shared.Next(9999)).ToString());
        form.Add("resolution_type", "4");
        var content = new FormUrlEncodedContent(form)
        {
            Headers = { { "md", MakeMd(uri) } }
        };
        return await _httpClient.PostAsync(uri, content);
    }

    public string MakeMd(string uri)
    {
        // HMACSHA256.ComputeHash is not thread-safe so create new HMACSHA256 every time
        // HMACSHA256 uses no unmanaged resources so it's unneeded to dispose it. 
        var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(uri));
        return Convert.ToBase64String(hash);
    }
}