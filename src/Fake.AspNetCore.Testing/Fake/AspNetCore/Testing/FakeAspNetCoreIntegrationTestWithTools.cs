using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Fake.Modularity;
using Microsoft.Net.Http.Headers;

namespace Fake.AspNetCore.Testing;

public class FakeAspNetCoreIntegrationTestWithTools<TStartupModule> : FakeAspNetCoreIntegrationTest<TStartupModule>
    where TStartupModule : class, IFakeModule
{
    protected virtual async Task<T> GetResponseAsync<T>(string url,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var strResponse = await GetResponseAsStringAsync(url, expectedStatusCode);
        return JsonSerializer.Deserialize<T>(strResponse,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    protected virtual async Task<string> GetResponseAsStringAsync(string url,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        using (var response = await GetResponseAsMessageAsync(url, expectedStatusCode))
        {
            return await response.Content.ReadAsStringAsync();
        }
    }

    protected virtual async Task<HttpResponseMessage> GetResponseAsMessageAsync(string url,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK, bool xmlHttpRequest = false)
    {
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
        {
            requestMessage.Headers.Add("Accept-Language", CultureInfo.CurrentUICulture.Name);
            if (xmlHttpRequest)
            {
                requestMessage.Headers.Add(HeaderNames.XRequestedWith, "XMLHttpRequest");
            }

            var response = await Client.SendAsync(requestMessage);
            Debug.Assert(response.StatusCode == expectedStatusCode, $"期望的状态码是 {expectedStatusCode}，但实际是 {response.StatusCode}。");
            return response;
        }
    }
}