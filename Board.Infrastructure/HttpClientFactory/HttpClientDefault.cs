using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Board.Infrastructure.HttpClientFactory
{
    public class HttpClientDefault : IHttpClient
    {
        public virtual async Task<TResult> GetAsync<T, TResult>(string url, Dictionary<string, string>? dicHeader = null, Dictionary<string, string>? dicParams = null)
            where T : class
            where TResult : class
        {
            try
            {     
                HttpClient client = HttpClientProvider.GetHttpClient();
                if (dicHeader != null && dicHeader.Count > 0)
                {
                    client.DefaultRequestHeaders.Clear();
                    foreach (var item in dicHeader)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                //执行请求
                var response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (!string.IsNullOrEmpty(result))
                        return JsonConvert.DeserializeObject<TResult>(result);
                    else
                        return default(TResult);
                }
                else
                {
                    if (string.IsNullOrEmpty(result))
                        result = response.ReasonPhrase;

                    throw new Exception(result);
                }
            }
            catch (Exception ea)
            {
                throw new Exception($"请求服务失败:{url}--->{ea.ToString()}");
            }
        }

        public virtual async Task<TResult> PostAsync<T, TResult>(string url, T obj, Dictionary<string, string>? dicHeader = null)
            where T : class
            where TResult : class
        {
            //string data = JsonConvert.SerializeObject(obj);
            string data = obj?.ToString();//JsonConvert.SerializeObject(obj);
            try
            {
                //post 参数
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpClient client = HttpClientProvider.GetHttpClient();
                if (dicHeader != null && dicHeader.Count > 0)
                {
                    client.DefaultRequestHeaders.Clear();
                    foreach (var item in dicHeader)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                //执行请求
                var response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (!string.IsNullOrEmpty(result))
                        return JsonConvert.DeserializeObject<TResult>(result);
                    else
                        return default(TResult);
                }
                else
                {
                    if (string.IsNullOrEmpty(result))
                        result = response.ReasonPhrase;

                    throw new Exception(result);
                }
            }
            catch (Exception ea)
            {
                throw new Exception($"请求服务失败:{url}--->{ea.ToString()}");
            }
        }

        public virtual async Task<TResult> PostOauthAsync<T, TResult>(string url, Dictionary<string, string>? dicHeader = null, Dictionary<string, string>? dicParams = null)
            where T : class
            where TResult : class
        {
            try
            {
                //form 参数
                var formContent = new FormUrlEncodedContent(dicParams);

                HttpClient client = HttpClientProvider.GetHttpClient();
                
                if (dicHeader != null && dicHeader.Count > 0)
                {
                    client.DefaultRequestHeaders.Clear();
                    foreach (var item in dicHeader)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                //执行请求
                var response = await client.PostAsync(url, formContent);
                string result = await response.Content.ReadAsStringAsync();

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    if (!string.IsNullOrEmpty(result))
                        return JsonConvert.DeserializeObject<TResult>(result);
                    else
                        return default(TResult);
                }
                else
                {
                    if (string.IsNullOrEmpty(result))
                        result = response.ReasonPhrase;

                    throw new Exception(result);
                }
            }
            catch (Exception ea)
            {
                throw new Exception($"请求服务失败:{url}--->{ea.ToString()}");
            }
        }
    }
}
