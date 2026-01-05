using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Arrays.Bytes;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.String;
using Soenneker.Extensions.ValueTask;
using Soenneker.NameCom.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.NameCom.Client;

/// <inheritdoc cref="INameComClientUtil"/>
public sealed class NameComClientUtil : INameComClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly string _username;
    private readonly string _token;

    private const string _prodBaseUrl = "https://api.name.com/v4/";
    private const string _testBaseUrl = "https://api.dev.name.com/v4/";
    private const string _clientId = nameof(NameComClientUtil);
    private const string _testClientId = nameof(NameComClientUtil) + "-test";

    public NameComClientUtil(IHttpClientCache httpClientCache, IConfiguration configuration)
    {
        _httpClientCache = httpClientCache;
        _username = configuration.GetValueStrict<string>("NameCom:Username");
        _token = configuration.GetValueStrict<string>("NameCom:Token");
    }

    public ValueTask<HttpClient> Get(bool test = false, CancellationToken cancellationToken = default)
    {
        return test
            ? _httpClientCache.Get(_testClientId, CreateTestHttpClientOptions, cancellationToken)
            : _httpClientCache.Get(_clientId, CreateProdHttpClientOptions, cancellationToken);
    }

    private HttpClientOptions? CreateProdHttpClientOptions()
    {
        string authHeader = $"{_username}:{_token}".ToBytes().ToBase64String();

        return new HttpClientOptions
        {
            BaseAddress = _prodBaseUrl,
            DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", $"Basic {authHeader}" }
            }
        };
    }

    private HttpClientOptions? CreateTestHttpClientOptions()
    {
        string username = _username + "-test";
        string authHeader = $"{username}:{_token}".ToBytes().ToBase64String();

        return new HttpClientOptions
        {
            BaseAddress = _testBaseUrl,
            DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", $"Basic {authHeader}" }
            }
        };
    }

    public void Dispose()
    {
        _httpClientCache.RemoveSync(_clientId);
        _httpClientCache.RemoveSync(_testClientId);
    }

    public async ValueTask DisposeAsync()
    {
        await _httpClientCache.Remove(_clientId).NoSync();
        await _httpClientCache.Remove(_testClientId).NoSync();
    }
}
