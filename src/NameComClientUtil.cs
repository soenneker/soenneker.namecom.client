using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.NameCom.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.NameCom.Client;

/// <inheritdoc cref="INameComClientUtil"/>
public class NameComClientUtil : INameComClientUtil
{
    private readonly IHttpClientCache _httpClientCache;

    public NameComClientUtil(IHttpClientCache httpClientCache)
    {
        _httpClientCache = httpClientCache;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(nameof(NameComClientUtil), null, cancellationToken);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _httpClientCache.RemoveSync(nameof(NameComClientUtil));
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return _httpClientCache.Remove(nameof(NameComClientUtil));
    }
}