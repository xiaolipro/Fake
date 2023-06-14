using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Fake.Http.Fake.Http
{
    public class FakeHttpOptions
    {
        public string BaseAddress { get; set; }
        public AuthenticationHeaderValue AuthenticationHeaderValue { get; set; }
        public string[] Accept { get; set; } =
        {
            "application/json",
            "text/plain",
            "*/*"
        };

        public KeyValuePair<string, string>[] Headers { get; set; }

        public bool EnableCompress { get; set; } = false;
    }
}
