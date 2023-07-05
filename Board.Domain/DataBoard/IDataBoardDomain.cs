using Board.Domain.Entities;
using Board.Infrastructure;

namespace Board.Domain.DataBoard
{
    public interface IDataBoardDomain : IBaseDomain
    {
        /// <summary>
        /// 获取T34仓库库位信息
        /// </summary>
        /// <returns></returns>
        public Location GetLocationInfo();

        public long GetLocationId();

        /// <summary>
        /// 待入任务
        /// </summary>
        /// <returns></returns>
        public List<InTask> GetInTask();


        /// <summary>
        /// 待出任务
        /// </summary>
        /// <returns></returns>
        public List<OutTask> GetOutTask();

        /// <summary>
        /// 成品待出货任务
        /// </summary>
        /// <returns></returns>
        public List<FGOutTask> GetFGOutTask();

        /// <summary>
        /// 成品入库计划达成进度
        /// </summary>
        /// <returns></returns>
        public List<FGInPlan> GetFGInPlan();

        /// <summary>
        /// 最近一周出入库数量
        /// </summary>
        /// <returns></returns>
        public List<InOutQty> GetInOutQtyWeek();

        /// <summary>
        /// 最新采集设备数据(温湿度)
        /// </summary>
        /// <returns></returns>
        public List<Humiture> GetHumiture();
    }
}
