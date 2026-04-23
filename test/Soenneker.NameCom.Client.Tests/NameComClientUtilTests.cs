using Soenneker.NameCom.Client.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.NameCom.Client.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class NameComClientUtilTests : HostedUnitTest
{
    private readonly INameComClientUtil _util;

    public NameComClientUtilTests(Host host) : base(host)
    {
        _util = Resolve<INameComClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
