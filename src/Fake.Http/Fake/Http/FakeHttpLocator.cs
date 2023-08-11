using Fake.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fake.Http.Fake.Http
{
    public static class FakeHttpLocator
    {
        private static FakeHttpOptions _option = new FakeHttpOptions();

        internal static void Instance(FakeHttpOptions option)
        {
            _option = option;
        }

        internal static void InitAuthentication(string key, string value)
        {
            _option.AuthenticationHeaderValue = new System.Net.Http.Headers.AuthenticationHeaderValue(key, value);
        }

        internal static void InitHeader(string key, string value)
        {
            var result = _option.Headers.ToList();
            result?.Add(new KeyValuePair<string, string>(key, value));

            _option.Headers = result.Distinct().ToArray();
        }

        internal static void AddFakeHttp(Action<FakeHttpOptions> setupAction = null)
        {
            var option = new FakeHttpOptions();

            setupAction?.Invoke(option);

            Instance(option);
        }

        internal static FakeHttpOptions GetOption()
        {
            return _option;
        }

        internal static IFakeJsonSerializer Serializer { get; private set; }
        internal static ILogger Logger { get; private set; }

        public static void UseFakeHttp(IFakeJsonSerializer serializer, ILogger logger = null)
        {
            Serializer = serializer;
            Logger = logger;
        }
    }
}

