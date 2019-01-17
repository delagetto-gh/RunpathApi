using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Runpath.Api
{
    public abstract class BaseHttpClient : IDisposable
    {
        private readonly string baseUri;

        private HttpClient httpClient;

        private HttpClient HttpClient
        {
            get
            {
                if (this.httpClient == null)
                {
                    this.httpClient = new HttpClient();
                    this.httpClient.BaseAddress = new Uri(this.baseUri);
                }
                return this.httpClient;
            }
        }

        protected BaseHttpClient(string baseUri)
        {
            this.baseUri = baseUri;
        }

        protected Task<HttpResponseMessage> Get(string uri)
        {
            var fullUri = this.GenerateRequestUri(uri);

            return this.SendRequest(HttpMethod.Get, fullUri);
        }

        private string GenerateRequestUri(params string[] uriParams)
        {
            var allUriParams = new List<string>() { this.baseUri };
            allUriParams.AddRange(uriParams);
            var fullUri = string.Join("/", allUriParams);
            return fullUri;
        }

        private Task<HttpResponseMessage> SendRequest(HttpMethod method, string uri)
        {
            var message = new HttpRequestMessage(method, uri);

            var response = this.HttpClient.SendAsync(message);

            return response;
        }

        public void Dispose()
        {
            this.HttpClient.Dispose();
        }
    }
}