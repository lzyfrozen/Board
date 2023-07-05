using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Infrastructure.HttpClientFactory
{
    public class FeedbackDto
    {
        public string PlatformNo { get; set; }

        public FeedbackTypes FeedbackType { get; set; }

        public string MessageId { get; set; } = Guid.NewGuid().ToString();

        public string ApiUrl { get; set; }
        public string Method { get; set; }

        public Dictionary<string, string> Header { get; set; } = null;

        public Dictionary<string, string> Params { get; set; } = null;

        public string Data { get; set; }

        public DateTime FeedbackTime { get; set; } = DateTime.Now;
    }
}
