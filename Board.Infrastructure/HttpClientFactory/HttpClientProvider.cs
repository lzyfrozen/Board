using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Board.Infrastructure.HttpClientFactory
{
    public class HttpClientProvider
    {
        private static HttpClient _client = null;
        static HttpClientProvider()
        {
            //压缩
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                // 如果服务器有 https 证书，但是证书不安全，则需要使用下面语句
                // => 也就是说，不校验证书，直接允许
                //ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true
            };

            _client = new HttpClient(handler);
            _client.Timeout = TimeSpan.FromSeconds(120);
        }

        public static HttpClient GetHttpClient()
        {
            return _client;
        }
    }
}
