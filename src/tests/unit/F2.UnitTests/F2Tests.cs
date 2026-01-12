
using Xunit;
using FluentAssertions;

public class F2UnitTests
{
    [Fact]
    public void ReturnsHelloText()
    {
        FunctionApp.f2.F2.BuildResponse().Should().Be("Hello from f2");
    }
}
