using Soenneker.NameCom.Client.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.NameCom.Client.Tests;

[Collection("Collection")]
public class NameComClientUtilTests : FixturedUnitTest
{
    private readonly INameComClientUtil _util;

    public NameComClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<INameComClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
