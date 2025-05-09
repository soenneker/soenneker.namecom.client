using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Arrays.Bytes;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.String;
using Soenneker.Extensions.ValueTask;
using Soenneker.NameCom.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.NameCom.Client;

/// <inheritdoc cref="INameComClientUtil"/>
public class NameComClientUtil : INameComClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;

    private const string _prodBaseUrl = "https://api.name.com/v4/";
    private const string _testBaseUrl = "https://api.dev.name.com/v4/";
    private const string _clientId = nameof(NameComClientUtil);
    private const string _testClientId = nameof(NameComClientUtil) + "-test";

    public NameComClientUtil(IHttpClientCache httpClientCache, IConfiguration configuration)
    {
        _httpClientCache = httpClientCache;
        _configuration = configuration;
    }

    public ValueTask<HttpClient> Get(bool test = false, CancellationToken cancellationToken = default)
    {
        string clientId = test ? _testClientId : _clientId;
        string baseUrl = test ? _testBaseUrl : _prodBaseUrl;

        return _httpClientCache.Get(clientId, () =>
        {
            var username = _configuration.GetValueStrict<string>("NameCom:Username");
            var token = _configuration.GetValueStrict<string>("NameCom:Token");

            if (test)
                username += "-test";

            string authHeader = $"{username}:{token}".ToBytes().ToBase64String();

            return new HttpClientOptions
            {
                BaseAddress = baseUrl,
                DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "Authorization", $"Basic {authHeader}" }
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _httpClientCache.RemoveSync(_clientId);
        _httpClientCache.RemoveSync(_testClientId);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _httpClientCache.Remove(_clientId).NoSync();
        await _httpClientCache.Remove(_testClientId).NoSync();
    }
}
