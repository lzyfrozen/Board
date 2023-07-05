using Board.Application;
using Board.Application.DataBoard.Dto;
using Board.Domain.Entities;
using Board.Infrastructure.Models;
using Board.ToolKits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Board.HttpApi.Controllers
{
    /// <summary>
    /// DataBoardController
    /// </summary>

    public class DataBoardController : BaseController
    {
        private readonly ILogger<DataBoardController> _logger;
        private readonly IDataBoardService _dataBoardService;
        //private static string BinEnable = AppSettings.app(new[] { "WareHouse", "BinEnable" });

        private static MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions() { });

        public DataBoardController(ILogger<DataBoardController> logger,
                                    IDataBoardService dataBoardService)
        {
            _logger = logger;
            _dataBoardService = dataBoardService;
        }


        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Test")]
        public ApiResult<dynamic> Test()
        {
            _logger.LogDebug("DataBoard-Test");
            //string jsonData = GetJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Resources/data.json"));
            //var data = JsonConvert.DeserializeObject(jsonData);
            var locationInfo = _dataBoardService.GetLocationInfo();
            var inTaskData = _dataBoardService.GetInTask();
            //入库任务数
            var inTaskQty = inTaskData.Count(x => x.事务类型 == "IN");
            var inTaskCompleteQty = inTaskData.Count(x => x.事务类型 == "99");

            var inTask = inTaskData.Where(x => x.事务类型 == "IN");
            var date = DateTime.Now;

            var result = new ApiResult<dynamic>()
            {
                Result = true,
                Data = new
                {
                    Data0 = new List<dynamic>() { new { 库位总数 = locationInfo.库位总数 } },
                    Data1 = new List<dynamic>() { new { 使用库位数量 = locationInfo.使用数量 } },
                    Data2 = new List<dynamic>() { new { 空仓库位数量 = locationInfo.空仓 } },
                    Data3 = new List<dynamic>() { new { 库存超期预警 = locationInfo.库存超期预警 } },
                    Data4 = new List<dynamic>() { new { 入库任务及完成数量 = "103-39" } },
                    Data5 = new List<dynamic>() { new { 出库任务及完成数量 = "237-154" } },
                    Data6 = new List<InTaskDto>() {
                            new InTaskDto { 序号 = 1, 入库单号 = "L2022112900012", 下发时间 = date },
                            new InTaskDto { 序号 = 2, 入库单号 = "L2022112900011", 下发时间 = date },
                            new InTaskDto { 序号 = 3, 入库单号 = "L2022112900010", 下发时间 = date },
                            new InTaskDto { 序号 = 4, 入库单号 = "L202211290009", 下发时间 = date },
                            new InTaskDto { 序号 = 5, 入库单号 = "L202211290008", 下发时间 = date },
                            new InTaskDto { 序号 = 6, 入库单号 = "L202211290007", 下发时间 = date },
                            new InTaskDto { 序号 = 7, 入库单号 = "L202211290006", 下发时间 = date },
                            new InTaskDto { 序号 = 8, 入库单号 = "L202211290005", 下发时间 = date },
                            new InTaskDto { 序号 = 9, 入库单号 = "L202211290004", 下发时间 = date },
                            new InTaskDto { 序号 = 10, 入库单号 = "L202211290003", 下发时间 = date },
                            new InTaskDto { 序号 = 11, 入库单号 = "L202211290002", 下发时间 = date },
                            new InTaskDto { 序号 = 12, 入库单号 = "L202211290001", 下发时间 = date }
                    },

                    Data7 = new List<OutTaskDto>() {
                            new OutTaskDto { 序号 = 1, 出库单号 = "C2022112900012", 下发时间 = date },
                            new OutTaskDto { 序号 = 2, 出库单号 = "C2022112900011", 下发时间 = date },
                            new OutTaskDto { 序号 = 3, 出库单号 = "C2022112900010", 下发时间 = date },
                            new OutTaskDto { 序号 = 4, 出库单号 = "C202211290009", 下发时间 = date },
                            new OutTaskDto { 序号 = 5, 出库单号 = "C202211290008", 下发时间 = date },
                            new OutTaskDto { 序号 = 6, 出库单号 = "C202211290007", 下发时间 = date },
                            new OutTaskDto { 序号 = 7, 出库单号 = "C202211290006", 下发时间 = date },
                            new OutTaskDto { 序号 = 8, 出库单号 = "C202211290005", 下发时间 = date },
                            new OutTaskDto { 序号 = 9, 出库单号 = "C202211290004", 下发时间 = date },
                            new OutTaskDto { 序号 = 10, 出库单号 = "C202211290003", 下发时间 = date },
                            new OutTaskDto { 序号 = 11, 出库单号 = "C202211290002", 下发时间 = date },
                            new OutTaskDto { 序号 = 12, 出库单号 = "C202211290001", 下发时间 = date }
                    },
                    Data8 = new List<HumitureDto>() {
                            new HumitureDto { 温度 = "12.6", 温湿度仪名称 = "95A-01", 温度预警 = "0", 连接状态 = "正常采集", 湿度预警 = "0", 湿度 = "42.6" },
                            new HumitureDto { 温度 = "12.9", 温湿度仪名称 = "95A-02", 温度预警 = "0", 连接状态 = "正常采集", 湿度预警 = "0", 湿度 = "44.2" },
                            new HumitureDto { 温度 = "13.1", 温湿度仪名称 = "95B-01", 温度预警 = "0", 连接状态 = "正常采集", 湿度预警 = "0", 湿度 = "43.8" },
                            new HumitureDto { 温度 = "13.5", 温湿度仪名称 = "95B-02", 温度预警 = "0", 连接状态 = "正常采集", 湿度预警 = "0", 湿度 = "38.9" },
                            new HumitureDto { 温度 = "13.4", 温湿度仪名称 = "95C-01", 温度预警 = "0", 连接状态 = "正常采集", 湿度预警 = "0", 湿度 = "41.5" },
                            new HumitureDto { 温度 = "13.9", 温湿度仪名称 = "95C-02", 温度预警 = "0", 连接状态 = "正常采集", 湿度预警 = "0", 湿度 = "41.4" },
                            new HumitureDto { 温度 = "18.8", 温湿度仪名称 = "T31-01", 温度预警 = "0", 连接状态 = "正常采集", 湿度预警 = "0", 湿度 = "29.6" },
                            new HumitureDto { 温度 = "17.8", 温湿度仪名称 = "T31-02", 温度预警 = "0", 连接状态 = "正常采集", 湿度预警 = "0", 湿度 = "36.5" }
                    },
                    Data9 = new List<FGOutTaskDto>() {
                            new FGOutTaskDto { 序号 = 1, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 2, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 3, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 4, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 5, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 6, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 7, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 8, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 9, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 10, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 11, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date },
                            new FGOutTaskDto { 序号 = 12, 物流类别 = "整车", 物料编码 = "91024492", 产品型号 = "PF0352", 客户ID = "JXJL", 出货车牌 = "", 库存数量 = "24", 卡板数量 = "14", 订货数量 = "24", 出货时间 = date }
                    },
                    Data10 = new List<FGInPlanDto>() {
                        new FGInPlanDto { 产品型号 = "PF0124", 计划 = "100", 完成 = "100" },
                        new FGInPlanDto { 产品型号 = "FP302A", 计划 = "20", 完成 = "20" },
                        new FGInPlanDto { 产品型号 = "FP233A", 计划 = "50", 完成 = "50" },
                        new FGInPlanDto { 产品型号 = "SKB01", 计划 = "1000", 完成 = "1000" },
                    },
                    Data11 = new List<InOutQtyDto>() {
                        new InOutQtyDto { 日期 = "23日", 入库数量 = 326444, 出库数量 = 171647 },
                        new InOutQtyDto { 日期 = "24日", 入库数量 = 63739, 出库数量 = 419147 },
                        new InOutQtyDto { 日期 = "25日", 入库数量 = 316325, 出库数量 = 354431 },
                        new InOutQtyDto { 日期 = "26日", 入库数量 = 969674, 出库数量 = 261778 },
                        new InOutQtyDto { 日期 = "27日", 入库数量 = 184014, 出库数量 = 374805 },
                        new InOutQtyDto { 日期 = "28日", 入库数量 = 114574, 出库数量 = 212666 },
                        new InOutQtyDto { 日期 = "29日", 入库数量 = 50050, 出库数量 = 131098 }
                    }
                }
            };
            return result;

        }

        /// <summary>
        /// GetKanbanList
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetKanbanList")]
        public ApiResult<dynamic> GetKanbanList()
        {
            _logger.LogDebug("DataBoard-GetKanbanList");

            var locationInfo = _dataBoardService.GetLocationInfo();

            string BinEnable = AppSettings.app(new[] { "WareHouse", "BinEnable" });
            string BinTotal = AppSettings.app(new[] { "WareHouse", "BinTotal" });//库位总数
            string BinUsed = AppSettings.app(new[] { "WareHouse", "BinUsed" });//库位使用

            //locationInfo.库位总数        locationInfo.使用数量    locationInfo.空仓
            var bBinEnable = BinEnable.ToUpper().Equals("FALSE") ? true : false;

            //总
            var locationTotal = bBinEnable ? int.Parse(BinTotal) : locationInfo.库位总数;
            //使用
            var binUsed = bBinEnable ? int.Parse(BinUsed) : locationInfo.使用数量;
            //空闲
            var binFree = bBinEnable ? int.Parse(BinTotal) - int.Parse(BinUsed) : locationInfo.空仓;

            //入库任务
            var inTaskData = _dataBoardService.GetInTask();
            var inTask = inTaskData.Where(x => x.事务类型 == "IN");//收货
            var inTaskComplete = inTaskData.Where(x => x.事务类型 == "99");//ASN关闭
            var inTaskDataList = inTask.Where(x => !inTaskComplete.Select(l => l.入库单号).Contains(x.入库单号))
                .OrderBy(l => l.下发时间)
                .Select((item, index) => { item.序号 = index + 1; return item; });
            //出库任务
            var outTaskData = _dataBoardService.GetOutTask();
            var outTask = outTaskData.Where(x => x.事务类型 == "PK");//拣货
            var outTaskComplete = outTaskData.Where(x => x.事务类型 == "99");//SO关闭
            var outTaskDataList = outTask.Where(x => !outTaskComplete.Select(l => l.出库单号).Contains(x.出库单号))
                .OrderBy(l => l.下发时间)
                .Select((item, index) => { item.序号 = index + 1; return item; });
            //成品待出库任务
            var fgOutTaskData = _dataBoardService.GetFGOutTask().Select((item, index) => { item.序号 = index + 1; return item; }); ;
            //成品入库计划达成进度
            var fgInPlanData = _dataBoardService.GetFGInPlan();
            //出入库趋势
            var inOutQtyWeekData = _dataBoardService.GetInOutQtyWeek();

            //温度湿度
            //var humiture = _dataBoardService.GetHumiture();
            var humiture = _dataBoardService.GetHumitureByDB();

            var result = new ApiResult<dynamic>()
            {
                Result = true,
                Data = new
                {
                    Data0 = new List<dynamic>() { new { 库位总数 = locationTotal } },
                    Data1 = new List<dynamic>() { new { 使用库位总数 = binUsed } },
                    Data2 = new List<dynamic>() { new { 空仓库位总数 = binFree } },
                    Data3 = new List<dynamic>() { new { 库存超期预警 = locationInfo.库存超期预警 } },
                    Data4 = new List<dynamic>() { new { 入库任务及完成数量 = $@"{inTask.Count()}-{inTaskComplete.Count()}" } },
                    Data5 = new List<dynamic>() { new { 出库任务及完成数量 = $@"{outTask.Count()}-{outTaskComplete.Count()}" } },
                    Data6 = inTaskDataList,
                    Data7 = outTaskDataList,
                    Data8 = humiture,
                    Data9 = fgOutTaskData,
                    Data10 = fgInPlanData,
                    Data11 = inOutQtyWeekData
                }
            };
            return result;
        }

        /// <summary>
        /// GetHumiture
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetHumiture")]
        public ApiResult<dynamic> GetHumiture()
        {
            _logger.LogDebug("DataBoard-GetHumiture");

            var humiture = new List<HumitureDto>();

            if (memoryCache.Get("Humiture") == null)
            {
                //温度湿度
                humiture = _dataBoardService.GetHumiture();
                memoryCache.Set("Humiture", humiture, TimeSpan.FromMinutes(2));
                //memoryCache.Set("Humiture", humiture, new MemoryCacheEntryOptions
                //{
                //    SlidingExpiration = TimeSpan.FromMinutes(10),
                //});
            }
            else
            {
                humiture = memoryCache.Get("Humiture") as List<HumitureDto>;
            }

            var result = new ApiResult<dynamic>()
            {
                Result = true,
                Data = new
                {

                    Data8 = humiture
                }
            };
            return result;
        }

        /// <summary>
        /// GetServerTime
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetServerTime")]
        public ApiResult<DateTime> GetServerTime()
        {
            var result = new ApiResult<DateTime>()
            {
                Result = true,
                Data = DateTime.Now
            };
            return result;
        }

        //public static string GetJsonFile(string filePath)
        //{
        //    string json = string.Empty;
        //    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        //    {
        //        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        //        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
        //        {
        //            json = sr.ReadToEnd().ToString();
        //        }
        //    }
        //    return json;
        //}
    }
}
