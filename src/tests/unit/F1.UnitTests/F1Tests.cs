
using Xunit;
using FluentAssertions;

public class F1UnitTests
{
    [Fact]
    public void ReturnsHelloText()
    {
        FunctionApp.f1.F1.BuildResponse().Should().Be("Hello from f1");
    }
}
