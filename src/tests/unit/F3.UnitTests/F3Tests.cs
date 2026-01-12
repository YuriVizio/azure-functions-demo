
using Xunit;
using FluentAssertions;

public class F3UnitTests
{
    [Fact]
    public void ReturnsHelloText()
    {
        FunctionApp.f3.F3.BuildResponse().Should().Be("Hello from f3");
    }
}
