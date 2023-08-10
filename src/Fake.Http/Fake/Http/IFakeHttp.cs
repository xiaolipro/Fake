using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fake.Http.Fake.Http
{
    public interface IFakeHttp
    {
        IFakeHttp Create();

        IFakeHttp Authentication(string scheme, string parameter);
        IFakeHttp Accept(string[] accept);
        IFakeHttp ContentType(string mediaType, string charSet = null);
        IFakeHttp Url(string url);
        IFakeHttp Params(object parameters);
        IFakeHttp Body(object body);
        IFakeHttp File(string path, string name, string filename);
        IFakeHttp File(string content, string name);
        IFakeHttp Form(IEnumerable<KeyValuePair<string, string>> nameValueCollection);
        IFakeHttp Headers(NameValueCollection headers);
        IFakeHttp Header(string key, string value);
        T GetData<T>(HttpResponseMessage res);
        Task<HttpResponseMessage> GetHttpResponseMessageAsync();
        HttpResponseMessage PostHttpResponseMessage();
        Task<HttpResponseMessage> PostHttpResponseMessageAsync();
        HttpResponseMessage PutHttpResponseMessage();
        Task<HttpResponseMessage> PutHttpResponseMessageAsync();
        HttpResponseMessage DeleteHttpResponseMessage();
        Task<HttpResponseMessage> DeleteHttpResponseMessageAsync();
        Task<HttpResponseMessage> PatchHttpResponseMessageAsync();
        HttpResponseMessage GetHttpResponseMessage();
        Task<T> GetAsync<T>();
        Task<T> PatchAsync<T>();
        Task<T> PostAsync<T>();
        Task<T> PutAsync<T>();
        Task<T> DeleteAsync<T>();
        string ToString();
        Task<bool> DownloadAsync(string path = null, string filename = null);
    }
}

