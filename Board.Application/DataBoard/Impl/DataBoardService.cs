using Board.Application.DataBoard.Dto;
using Board.Domain.DataBoard;
using Board.Domain.Entities;
using Board.Infrastructure;
using Board.Infrastructure.HttpClientFactory;
using Board.ToolKits;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Board.Application.Impl
{
    public class DataBoardService : IDataBoardService
    {
        private readonly ILogger<DataBoardService> _logger;
        private readonly IDataBoardDomain _dataBoardDomain;
        private readonly IEdiRequest _ediRequest;
        public DataBoardService(ILogger<DataBoardService> logger,
                                IDataBoardDomain dataBoardDomain,
                                IEdiRequest ediRequest)
        {
            _logger = logger;
            _dataBoardDomain = dataBoardDomain;
            _ediRequest = ediRequest;
        }

        public LocationDto GetLocationInfo()
        {
            var data = _dataBoardDomain.GetLocationInfo();
            return data.MapTo<LocationDto>();
        }
        public long GetLocationId()
        {
            var data = _dataBoardDomain.GetLocationId();
            return data;
        }

        public List<InTaskDto> GetInTask()
        {
            var data = _dataBoardDomain.GetInTask();
            return data.MapTo<List<InTaskDto>>();
        }

        public List<OutTaskDto> GetOutTask()
        {
            var data = _dataBoardDomain.GetOutTask();
            return data.MapTo<List<OutTaskDto>>();
        }

        public List<FGOutTaskDto> GetFGOutTask()
        {
            var data = _dataBoardDomain.GetFGOutTask();
            return data.MapTo<List<FGOutTaskDto>>();
        }

        public List<FGInPlanDto> GetFGInPlan()
        {
            var data = _dataBoardDomain.GetFGInPlan();
            return data.MapTo<List<FGInPlanDto>>();
        }

        public List<InOutQtyDto> GetInOutQtyWeek()
        {
            var data = _dataBoardDomain.GetInOutQtyWeek();

            var lstDate = new List<InOutQtyDto>();
            for (int i = 0; i < 7; i++)
            {
                lstDate.Add(new InOutQtyDto { 日期 = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd"), 天数 = DateTime.Today.AddDays(-i).Day.ToString("") });
            }
            foreach (var item in lstDate)
            {
                item.入库数量 = data.Any(l => l.Dates == item.日期 && l.InOutType == "IN") ? data.FirstOrDefault(l => l.Dates == item.日期 && l.InOutType == "IN")?.Qty : 0m;
                item.出库数量 = data.Any(l => l.Dates == item.日期 && l.InOutType == "OUT") ? data.FirstOrDefault(l => l.Dates == item.日期 && l.InOutType == "OUT")?.Qty : 0m;
                item.天数 = $"{item.天数}日";
            }
            return lstDate.OrderBy(l => l.日期).ToList();
        }

        public List<HumitureDto> GetHumiture()
        {
            var result = new List<HumitureDto>();

            var dto = new FeedbackDto();
            dto.PlatformNo = "PL";
            dto.ApiUrl = Path.Combine(AppSettings.app(new string[] { "PL", "ApiUrl" }), "kanban/getKanbanList");
            dto.FeedbackType = FeedbackTypes.朋乐;

            try
            {
                var res = _ediRequest.Get<PLResultDto>(dto);
                result = res.data.data8;
            }
            catch (Exception ex)
            {
                _logger.LogError($"接口:{dto.ApiUrl},获取数据失败,请检查!", ex);
            }
            return result;
        }

        public List<HumitureDto> GetHumitureByDB()
        {
            var data = _dataBoardDomain.GetHumiture();
            var result = data.MapTo<List<HumitureDto>>();

            result.ForEach(l =>
            {
                l.连接状态 = "正常采集";
                l.湿度预警 = (decimal.Parse(l.湿度) > decimal.Parse(l.湿度上限??"70") || decimal.Parse(l.湿度) < decimal.Parse(l.湿度下限?? "0")) ? "1" : "0";
                l.温度预警 = (decimal.Parse(l.温度) > decimal.Parse(l.温度上限 ?? "30") || decimal.Parse(l.湿度) < decimal.Parse(l.温度下限 ?? "5")) ? "1" : "0";
            });

            return result;
        }
    }
}
