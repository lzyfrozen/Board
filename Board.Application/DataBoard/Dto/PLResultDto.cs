using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Application.DataBoard.Dto
{
    public class PLResultDto
    {
        public bool result { get; set; }
        public string msg { get; set; }
        public string status { get; set; }
        public long count { get; set; }

        public KanBanData data { get; set; }
    }

    public class KanBanData
    {
        public List<HumitureDto> data8 { get; set; } = new List<HumitureDto>();
    }

}
