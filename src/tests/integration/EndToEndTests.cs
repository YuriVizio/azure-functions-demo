
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System;

public class EndToEndTests
{
    private static readonly string port = Environment.GetEnvironmentVariable("APP_PORT") ?? "8444";
    private static readonly HttpClient _http = new HttpClient { BaseAddress = new Uri($"http://localhost:{port}") };

    [Theory]
    [InlineData("/api/f1", "Hello from f1")]
    [InlineData("/api/f2", "Hello from f2")]
    [InlineData("/api/f3", "Hello from f3")]
    public async Task Functions_RespondHello(string route, string expected)
    {
        var resp = await _http.GetAsync(route);
        resp.IsSuccessStatusCode.Should().BeTrue();
        var text = await resp.Content.ReadAsStringAsync();
        text.Should().Be(expected);
    }
}
