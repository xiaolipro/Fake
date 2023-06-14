using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fake.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fake.Http.Fake.Http
{
    public class FakeHttp
    {
        private static readonly HttpClientHandler HttpClientHandler = new HttpClientHandler
        {
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            MaxAutomaticRedirections = 5,
        };

        private IFakeJsonSerializer Serializer => FakeHttpLocator.Serializer;
        private ILogger<FakeHttp> Logger => FakeHttpLocator.Logger;

        private static readonly HttpClient HttpClientInner = new HttpClient(HttpClientHandler);

        private static readonly MediaTypeHeaderValue DefaultMediaType = new MediaTypeHeaderValue("application/json");

        private static readonly string[] DefaultAccept =
        {
            "application/json",
            "text/plain",
            "*/*"
        };

        private static readonly HttpMethod PatchMethod = new HttpMethod("PATCH");

        private FakeHttpOptions Option => FakeHttpLocator.GetOption();

        private static readonly string[] DefaultAcceptEncoding = { "gzip", "deflate" };

        static FakeHttp()
        {
            try
            {
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.DefaultConnectionLimit = 200;
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                HttpClientInner.Timeout = TimeSpan.FromSeconds(60);
                _ = HttpClientInner.GetAsync("http://127.0.0.1").GetAwaiter().GetResult(); ;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 启用请求内容gzip压缩 自动使用gzip压缩body并设置Content-Encoding为gzip
        /// </summary>
        public static bool EnableCompress { get; set; } = false;

        private string _url;
        private HttpContent _httpContent;
        private NameValueCollection _headers;
        private MediaTypeHeaderValue _mediaType;
        private string[] _accept;
        private AuthenticationHeaderValue _authenticationHeaderValue;

        private Dictionary<string, object> _params;

        protected FakeHttp()
        {
            _mediaType = DefaultMediaType;
            _accept = DefaultAccept;
        }

        public static FakeHttp Create()
        {
            return new FakeHttp();
        }

        protected FakeHttp Authentication(AuthenticationHeaderValue authentication)
        {
            _authenticationHeaderValue = authentication;
            return this;
        }

        public FakeHttp Authentication(string scheme, string parameter)
        {
            _authenticationHeaderValue = new AuthenticationHeaderValue(scheme, parameter);
            return this;
        }

        /// <summary>
        /// 默认为 application/json, text/plain, */*
        /// </summary>
        /// <param name="accept"> </param>
        /// <returns> </returns>
        public FakeHttp Accept(string[] accept)
        {
            _accept = accept;
            return this;
        }

        public FakeHttp ContentType(string mediaType, string charSet = null)
        {
            _mediaType = new MediaTypeHeaderValue(mediaType);
            if (!string.IsNullOrEmpty(charSet))
            {
                _mediaType.CharSet = charSet;
            }

            return this;
        }

        /// <summary>
        /// 默认为 gzip, deflate
        /// </summary>
        /// <returns> </returns>
        /// public HttpClientWrapper AcceptEncoding(string[] acceptEncoding) { //_acceptEncoding =
        /// acceptEncoding; return this; }
        public FakeHttp Url(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            _url = url;
            return this;
        }

        public FakeHttp Params(object parameters)
        {
            _params = parameters.GetParameters();

            return this;
        }

        public FakeHttp Body(object body)
        {
            return Body(Serializer.Serialize(body));
        }

        public FakeHttp Body(string body)
        {
            if (string.IsNullOrEmpty(body))
            {
                return this;
            }

            var sc = new StringContent(body);
            if (EnableCompress)
            {
                _httpContent = new CompressedContent(sc, CompressedContent.CompressionMethod.GZip);
                sc.Headers.ContentEncoding.Add("gzip");
            }
            else
            {
                _httpContent = sc;
            }

            //sc.Headers.ContentLength = sc..Length;
            sc.Headers.ContentType = _mediaType;

            return this;
        }

        /// <summary>
        /// 仅支持POST和PUT
        /// </summary>
        /// <param name="path">     </param>
        /// <param name="name">     </param>
        /// <param name="filename"> </param>
        /// <returns> </returns>
        public FakeHttp File(string path, string name, string filename)
        {
            _httpContent = new MultipartFormDataContent();

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            var bac = new StreamContent(stream);
            ((MultipartFormDataContent)_httpContent).Add(bac, name, filename);

            return this;
        }

        /// <summary>
        /// 仅支持POST和PUT
        /// </summary>
        /// <param name="content"> </param>
        /// <param name="name">    </param>
        /// <returns> </returns>
        public FakeHttp File(string content, string name)
        {
            _httpContent = new MultipartFormDataContent();

            ((MultipartFormDataContent)_httpContent).Add(new StringContent(content), name);
            return this;
        }

        public FakeHttp Form(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            _httpContent = new FormUrlEncodedContent(nameValueCollection);

            return this;
        }

        public FakeHttp Headers(NameValueCollection headers)
        {
            CheckHeaderIsNull();

            foreach (string key in headers.Keys)
            {
                _headers.Add(key, headers.Get(key));
            }

            return this;
        }

        private void CheckHeaderIsNull()
        {
            _headers = new NameValueCollection();
        }

        public FakeHttp Header(string key, string value)
        {
            CheckHeaderIsNull();
            _headers.Add(key, value);
            return this;
        }

        public T GetData<T>(HttpResponseMessage res)
        {
            string str = null;

            Task.Run(async () => str = await res.Content.ReadAsStringAsync()).Wait();

            if (IsStringOrDecimalOrPrimitiveType(typeof(T)))
            {
                return (T)Convert.ChangeType(str, typeof(T));
            }

            if (string.IsNullOrEmpty(str)) throw new InvalidOperationException("请求异常");

            return Serializer.Deserialize<T>(str);
        }

        protected async Task<T> RequestAsync<T>(HttpMethod method)
        {
            using (var res = await GetOriginHttpResponseAsync(method))
            {
                return GetData<T>(res);
            }
        }

        private bool IsStringOrDecimalOrPrimitiveType(Type t)
        {
            var typename = t.Name;
            return t.IsPrimitive || typename.Equals("String") || typename.Equals("Decimal");
        }

        protected virtual async Task<HttpResponseMessage> GetOriginHttpResponseAsync(HttpMethod method)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var sw = new Stopwatch();

            var realUrl = Uri.EscapeUriString(_params is null ? _url : _params.ToUrl(_url));
            try
            {
                using (var requestMessage = new HttpRequestMessage(method, realUrl))
                {
                    switch (method.Method)
                    {
                        case "PUT":
                        case "POST":
                        case "PATCH":
                            if (_httpContent != null)
                            {
                                requestMessage.Content = _httpContent;
                            }
                            break;
                    }

                    foreach (var acc in (Option.Accept.Union(_accept)))
                    {
                        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acc));
                    }

                    foreach (var accenc in DefaultAcceptEncoding)
                    {
                        requestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(accenc));
                    }

                    if (_authenticationHeaderValue != null)
                    {
                        if (Option?.AuthenticationHeaderValue != null)
                        {
                            requestMessage.Headers.Authorization = Option.AuthenticationHeaderValue;
                        }

                        requestMessage.Headers.Authorization = _authenticationHeaderValue;
                    }

                    if (_headers != null)
                    {
                        if (Option?.Headers is null)
                        {
                            foreach (var header in _headers.AllKeys)
                            {
                                requestMessage.Headers.Add(header, _headers.Get(header));
                            }
                        }
                        else
                        {
                            foreach (var header in Option.Headers)
                            {
                                requestMessage.Headers.Add(header.Key, header.Value);
                            }
                        }
                    }

                    sw.Start();
                    HttpResponseMessage res = await HttpClientInner.SendAsync(requestMessage);
                    httpStatusCode = res.StatusCode;
                    sw.Stop();
                    return res;
                }

            }
            catch (Exception e)
            {
                Logger?.LogError(e.Message, e);

                throw new Exception(e.Message);
            }
            finally
            {
                Trace.WriteLine($"FakeHttp: {method.Method} {_url} {httpStatusCode} {sw.ElapsedMilliseconds}ms");
            }
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> GetHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponseAsync(HttpMethod.Get);
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public HttpResponseMessage PostHttpResponseMessage()
        {
            return GetOriginHttpResponseAsync(HttpMethod.Post).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> PostHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponseAsync(HttpMethod.Post);
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public HttpResponseMessage PutHttpResponseMessage()
        {
            return GetOriginHttpResponseAsync(HttpMethod.Put).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> PutHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponseAsync(HttpMethod.Put);
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public HttpResponseMessage DeleteHttpResponseMessage()
        {
            return GetOriginHttpResponseAsync(HttpMethod.Delete).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> DeleteHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponseAsync(HttpMethod.Delete);
        }

        public async Task<HttpResponseMessage> PatchHttpResponseMessageAsync()
        {
            return await GetOriginHttpResponseAsync(PatchMethod);
        }

        /// <summary>
        /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
        /// </summary>
        /// <returns> </returns>
        public HttpResponseMessage GetHttpResponseMessage()
        {
            return GetOriginHttpResponseAsync(HttpMethod.Get).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 自动将请求返回值反序列化为 <typeparam name="T"> </typeparam> 类型 若返回响应体无法反序列化则抛出异常
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public async Task<T> GetAsync<T>()
        {
            return await RequestAsync<T>(HttpMethod.Get);
        }

        [Obsolete("建议使用异步方法")]
        public T Get<T>()
        {
            return GetAsync<T>().GetAwaiter().GetResult();
        }

        public async Task<T> PatchAsync<T>()
        {
            return await RequestAsync<T>(PatchMethod);
        }

        [Obsolete("建议使用异步方法")]
        public T Patch<T>()
        {
            return PatchAsync<T>().GetAwaiter().GetResult();
        }

        public Task<T> PostAsync<T>()
        {
            return RequestAsync<T>(HttpMethod.Post);
        }

        [Obsolete("建议使用异步方法")]
        public T Post<T>()
        {
            return PostAsync<T>().GetAwaiter().GetResult();
        }

        public Task<T> PutAsync<T>()
        {
            return RequestAsync<T>(HttpMethod.Put);
        }

        [Obsolete("建议使用异步方法")]
        public T Put<T>()
        {
            return PutAsync<T>().GetAwaiter().GetResult();
        }

        public Task<T> DeleteAsync<T>()
        {
            return RequestAsync<T>(HttpMethod.Delete);
        }

        [Obsolete("建议使用异步方法")]
        public T Delete<T>()
        {
            return DeleteAsync<T>().GetAwaiter().GetResult();
        }

        public override string ToString()
        {
            return $"{_url}";
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="filename">文件名, 不包含后缀, 不填写时为GUID</param>
        /// <returns></returns>
        public async Task<bool> DownloadAsync(string path = null, string filename = null)
        {
            var response = await GetOriginHttpResponseAsync(HttpMethod.Get);

            if (!response.IsSuccessStatusCode) return false;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                string extension = Path.GetExtension(response.RequestMessage.RequestUri.ToString());

                if (!string.IsNullOrWhiteSpace(path))
                {
                    if (Directory.Exists(path))
                    {
                        try
                        {
                            Directory.CreateDirectory(path);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }

                using (var fs = new FileStream(
                           $"{path ?? Directory.GetCurrentDirectory()}/{filename ?? Guid.NewGuid().ToString()}{extension}",
                           FileMode.CreateNew))
                {
                    await stream.CopyToAsync(fs);
                }

            }

            return true;
        }

        public bool Download(string path = null, string filename = null)
        {
            return DownloadAsync(path, filename).GetAwaiter().GetResult();
        }
    }

    public class QFakeHttp
    {
        public static FakeHttp Url(string url)
        {
            return FakeHttp.Create().Url(url);
        }

        public static FakeHttp Uri(string uri)
        {
            var baseUrl = FakeHttpLocator.GetOption()?.BaseAddress;
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("没有配置 BaseAddress 不能使用 Uri 模式");
            }

            return FakeHttp.Create().Url($"{baseUrl}{(baseUrl.EndsWith("/") || uri.StartsWith("/") ? "" : "/")}{uri}");
        }

        public static void InitAuthentication(string key, string value)
        {
            FakeHttpLocator.InitAuthentication(key, value);
        }

        public static void AddFakeHttp(Action<FakeHttpOptions> options)
        {
            FakeHttpLocator.AddFakeHttp(options);
        }

        public static void AddHeader(string key, string value)
        {
            FakeHttpLocator.InitHeader(key, value);
        }

        public static async Task<T> GetAsync<T>(string url)
        {
            return await Url(url).GetAsync<T>();
        }
    }
}