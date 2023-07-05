using System.Net;

namespace Board.Infrastructure.Models
{
    public class ApiResult<T>
    {
        public bool Result { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public DateTime CurrentTime { get; set; } = DateTime.Now;

        public static implicit operator ApiResult<T>(ApiResult<object> v)
        {
            throw new NotImplementedException();
        }
    }
}
