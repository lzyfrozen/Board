using Board.Application.DataBoard.Dto;
using Board.Domain.Entities;
using Board.Infrastructure;

namespace Board.Application
{
    public interface IDataBoardService : IBaseService
    {
        /// <summary>
        /// 获取T34仓库库位信息
        /// </summary>
        /// <returns></returns>
        public LocationDto GetLocationInfo();

        public long GetLocationId();

        /// <summary>
        /// 待入任务
        /// </summary>
        /// <returns></returns>
        public List<InTaskDto> GetInTask();

        /// <summary>
        /// 待出任务
        /// </summary>
        /// <returns></returns>
        public List<OutTaskDto> GetOutTask();

        /// <summary>
        /// 成品待出货任务
        /// </summary>
        /// <returns></returns>
        public List<FGOutTaskDto> GetFGOutTask();

        /// <summary>
        /// 成品入库计划达成进度
        /// </summary>
        /// <returns></returns>
        public List<FGInPlanDto> GetFGInPlan();

        /// <summary>
        /// 最近一周出入库数量
        /// </summary>
        /// <returns></returns>
        public List<InOutQtyDto> GetInOutQtyWeek();

        /// <summary>
        /// 温度湿度
        /// </summary>
        /// <returns></returns>
        public List<HumitureDto> GetHumiture();
        public List<HumitureDto> GetHumitureByDB();

    }
}
