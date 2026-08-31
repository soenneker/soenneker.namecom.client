using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.NameCom.Client.Abstract;

/// <summary>
/// Provides cached, authenticated HTTP clients for Name.com's production and test APIs.
/// </summary>
public interface INameComClientUtil : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the production or test Name.com client.
    /// </summary>
    /// <param name="test">Whether to use Name.com's test API.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The cached client.</returns>
    ValueTask<HttpClient> Get(bool test = false, CancellationToken cancellationToken = default);
}
