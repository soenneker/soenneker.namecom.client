using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Extensions.Arrays.Bytes;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.String;
using Soenneker.Extensions.ValueTask;
using Soenneker.NameCom.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.HttpClientCache.Dtos;

namespace Soenneker.NameCom.Client;

/// <inheritdoc cref="INameComClientUtil"/>
public class NameComClientUtil : INameComClientUtil
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _configuration;

    private const string _prodBaseUrl = "https://api.name.com/v4/";
    private const string _testBaseUrl = "https://api.dev.name.com/v4/";

    public NameComClientUtil(IHttpClientCache httpClientCache, IConfiguration configuration)
    {
        _httpClientCache = httpClientCache;
        _configuration = configuration;
    }

    public ValueTask<HttpClient> Get(bool test = false, CancellationToken cancellationToken = default)
    {
        var username = _configuration.GetValueStrict<string>("NameCom:Username");
        var token = _configuration.GetValueStrict<string>("NameCom:Token");

        if (test)
            username += "-test";

        string baseUrl = test ? _testBaseUrl : _prodBaseUrl;

        string authHeader = $"{username}:{token}".ToBytes().ToBase64String();

        var options = new HttpClientOptions
        {
            BaseAddress = baseUrl,
            DefaultRequestHeaders = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", $"Basic {authHeader}" }
            }
        };

        string clientName = test ? $"{nameof(NameComClientUtil)}-test" : nameof(NameComClientUtil);

        return _httpClientCache.Get(clientName, options, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _httpClientCache.RemoveSync(nameof(NameComClientUtil));
        _httpClientCache.RemoveSync($"{nameof(NameComClientUtil)}-test");
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _httpClientCache.Remove(nameof(NameComClientUtil)).NoSync();
        await _httpClientCache.Remove($"{nameof(NameComClientUtil)}-test").NoSync();
    }
}
