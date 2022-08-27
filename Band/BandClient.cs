using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Band.Models;
using Band.Models.Comments;
using Band.Models.Feed;
using Band.Models.Posts;
using Polly;
using static Band.Helper;

namespace Band;

public class BandClient : IDisposable
{
    private const string UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Safari/537.36";

    public readonly CookieContainer CookieContainer;

    public readonly HttpClient HttpClient;
    public readonly HttpMessageHandler HttpMessageHandler;

    // We will set SecretKey in RefreshAsync() so suppress the non-initialized warning
    public volatile string SecretKey = null!;

    public string Suffix = "Sent from BAND Client Library";

    public BandClient(string cookies, string? proxy = null)
    {
        #region Initialize CookieContainer

        CookieContainer = new CookieContainer();
        var cookieList = ParseCookies(cookies);
        foreach (var cookie in cookieList)
        {
            cookie.Path = "/";
            cookie.Domain = ".band.us";
            CookieContainer.Add(cookie);
        }

        #endregion

        #region Initialize HttpClientHandler

        HttpMessageHandler = new HttpRetryMessageHandler(new HttpClientHandler
        {
            CookieContainer = CookieContainer,
            Proxy = new WebProxy(proxy)
        });

        #endregion

        #region Initialize HttpClient

        HttpClient = new HttpClient(HttpMessageHandler)
        {
            DefaultRequestHeaders =
            {
                {"user-agent", UserAgent},
                {"akey", "bbc59b0b5f7a1c6efe950f6236ccda35"},
                {"device-time-zone-id", "Asia/Shanghai"}
            },
            BaseAddress = new Uri("https://api.band.us/")
        };

        #endregion

        RefreshAsync().Wait();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<HttpResponseMessage> RefreshAsync()
    {
        var response = await HttpClient.GetAsync("https://auth.band.us/refresh");
        var secretKey = CookieContainer.GetAllCookies().FirstOrDefault(c => c.Name == "secretKey")?.Value;
        if (string.IsNullOrEmpty(secretKey)) throw new AuthenticationException("Cookies expired.");

        SecretKey = secretKey.Trim('"');
        return response;
    }

    public Task<HttpResponseMessage> CreateCommentAsync(int bandNo, int postNo, string body)
    {
        const string uri = "/v2.3.0/create_comment";
        var form = new CreateComment
        {
            BandNo = bandNo,
            Body = $"{body}\n{Suffix}",
            ContentKey = new ContentKey {PostNo = postNo}
        }.ToDictionary();
        return PostAsync(uri, form);
    }

    public Task<HttpResponseMessage> CreateCommentAsync(int bandNo, int postNo, int commentId, string body, int userNo,
        string userName)
    {
        const string uri = "/v2.3.0/create_comment";
        var form = new CreateComment
        {
            BandNo = bandNo,
            Body = $"<band:refer user_no=\"{userNo}\">{userName}</band:refer> {body}\n\n{Suffix}",
            ContentKey = new ContentKey {PostNo = postNo, ContentType = "post_comment", CommentId = commentId}
        }.ToDictionary();
        return PostAsync(uri, form);
    }

    public async Task<Posts> GetPostsAsync(int bandNo)
    {
        const string uri = "/v2.0.0/get_posts";
        var form = new
        {
            BandNo = bandNo,
            Direction = "before"
        }.ToDictionary();
        using var response = await PostAsync(uri, form);
        await using var stream = await response.Content.ReadAsStreamAsync();
        return (await JsonSerializer.DeserializeAsync<Result<Posts>>(stream))!.ResultData;
    }

    public async Task<Comments> GetCommentsAsync(int bandNo, int postNo)
    {
        const string uri = "/v2.3.0/get_comments";
        var form = new
        {
            BandNo = bandNo,
            ContentKey = new ContentKey {ContentType = "post", PostNo = postNo}
        }.ToDictionary();
        using var response = await PostAsync(uri, form);
        await using var stream = await response.Content.ReadAsStreamAsync();
        return (await JsonSerializer.DeserializeAsync<Result<Comments>>(stream))!.ResultData;
    }

    public async Task<Feed> GetFeedAsync()
    {
        const string uri = "/v2.1.0/get_feed";
        var form = new GetFeed().ToDictionary();
        using var response = await PostAsync(uri, form);
        await using var stream = await response.Content.ReadAsStreamAsync();
        return (await JsonSerializer.DeserializeAsync<Result<Feed>>(stream))!.ResultData;
    }

    public Task<HttpResponseMessage> PostAsync(string uri, IDictionary<string, string> form)
    {
        form.Add("ts", GetUnixTimeStamp().ToString());
        form.Add("resolution_type", "4");
        var content = new FormUrlEncodedContent(form)
        {
            Headers = {{"md", MakeMd(uri)}}
        };
        return HttpClient.PostAsync(uri, content);
    }

    public string MakeMd(string uri)
    {
        // HMACSHA256.ComputeHash is not thread-safe so create new HMACSHA256 every time
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(uri));
        return Convert.ToBase64String(hash);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        HttpClient.Dispose();
        HttpMessageHandler.Dispose();
    }

    public class HttpRetryMessageHandler : DelegatingHandler
    {
        public HttpRetryMessageHandler(HttpClientHandler handler) : base(handler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                })
                .ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }
    }
}