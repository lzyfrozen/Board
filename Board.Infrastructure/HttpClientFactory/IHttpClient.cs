using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Infrastructure.HttpClientFactory
{
    public interface IHttpClient
    {
        Task<TResult> GetAsync<T, TResult>(string url, Dictionary<string, string>? dicHeader = null, Dictionary<string, string>? dicParams = null) where T : class where TResult : class;

        Task<TResult> PostAsync<T, TResult>(string url, T obj, Dictionary<string, string>? dicHeader = null) where T : class where TResult : class;

        Task<TResult> PostOauthAsync<T, TResult>(string url, Dictionary<string, string>? dicHeader = null, Dictionary<string, string>? dicParams = null) where T : class where TResult : class;
    }
}
