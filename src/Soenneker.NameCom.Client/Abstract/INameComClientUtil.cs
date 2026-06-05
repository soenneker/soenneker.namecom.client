using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.NameCom.Client.Abstract;

/// <summary>
/// A .NET thread-safe singleton HttpClient for Name.com
/// </summary>
public interface INameComClientUtil : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="test">The test.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<HttpClient> Get(bool test = false, CancellationToken cancellationToken = default);
}
