namespace Straddle.UnitTests;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

public abstract class BaseUnitTest<Tsut> where Tsut : class
{
    protected MockRepository? MockRepository;
    protected Mock<ILogger<Tsut>> MockLogger = new();

    [TestCleanup]
    public void Cleanup()
    {
        MockRepository!.VerifyAll();
    }

    public Mock<T> Create<T>() where T : class
    {
        return MockRepository!.Create<T>();
    }

    public virtual void Initialize() { }

    [TestInitialize]
    public void InitTestRunner()
    {
        MockRepository = new MockRepository(MockBehavior.Strict);
        MockLogger = new Mock<ILogger<Tsut>>();
        Initialize();
    }
}