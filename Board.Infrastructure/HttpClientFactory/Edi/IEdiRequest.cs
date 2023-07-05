using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Infrastructure.HttpClientFactory
{
    public interface IEdiRequest
    {
        TResult Get<TResult>(FeedbackDto dto) where TResult : class;

        TResult Feedback<TResult>(FeedbackDto dto) where TResult : class;

        TResult Oauth<TResult>(FeedbackDto dto) where TResult : class;
    }
}
