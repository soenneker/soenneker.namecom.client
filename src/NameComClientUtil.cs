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
        // No closure: state passed explicitly + static lambda
        return test
            ? _httpClientCache.Get(_testClientId, (username: _username, token: _token, baseUrl: _testBaseUrl), static state =>
            {
                string username = state.username + "-test";
                string authHeader = $"{username}:{state.token}".ToBytes().ToBase64String();

                return new HttpClientOptions
                {
                    BaseAddress = state.baseUrl,
                    DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "Authorization", $"Basic {authHeader}" }
                    }
                };
            }, cancellationToken)
            : _httpClientCache.Get(_clientId, (username: _username, token: _token, baseUrl: _prodBaseUrl), static state =>
            {
                string authHeader = $"{state.username}:{state.token}".ToBytes().ToBase64String();

                return new HttpClientOptions
                {
                    BaseAddress = state.baseUrl,
                    DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "Authorization", $"Basic {authHeader}" }
                    }
                };
            }, cancellationToken);
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
