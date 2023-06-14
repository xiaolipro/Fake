using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Fake.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fake.Http.Fake.Http
{
    public static class FakeHttpExtensions
    {
        public static Dictionary<string, object> GetParameters(this object parameters)
        {
            var result = new Dictionary<string, object>();

            var properties = parameters.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                result.TryAdd(propertyInfo.Name, propertyInfo.GetValue(parameters));
            }

            return result;
        }

        public static string ToUrl(this Dictionary<string, object> dict, string url)
        {
            var sb = new StringBuilder();
            foreach (var kvp in dict)
            {
                if (kvp.Value is Array array)
                {
                    foreach (var item in array)
                    {
                        sb.Append($"&{kvp.Key}={item}");
                    }
                }
                else if (kvp.Value is IList list)
                {
                    foreach (var item in list)
                    {
                        sb.Append($"&{kvp.Key}={item}");
                    }
                }
                else
                {
                    sb.Append($"&{kvp.Key}={kvp.Value}");
                }
            }

            return $"{url.TrimEnd('/')}{sb.ToString().TrimStart('&')}";
        }

        public static void AddFakeHttp(this IServiceCollection services, Action<FakeHttpOptions> setupAction = null)
        {
            var option = new FakeHttpOptions();

            setupAction?.Invoke(option);

            FakeHttpLocator.Instance(option);
        }
    }
}