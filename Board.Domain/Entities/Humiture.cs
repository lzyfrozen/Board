using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Domain.Entities
{
    /// <summary>
    /// 温湿度
    /// </summary>
    public class Humiture
    {
        /// <summary>
        /// 采集设备
        /// </summary>
        public string? deviceId { get; set; }

        /// <summary>
        /// 采集地点
        /// </summary>
        public string? deviceName { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public string? collectionTime { get; set;}

        /// <summary>
        /// 湿度
        /// </summary>
        public string? humidity { get; set;}

        /// <summary>
        /// 湿度下限
        /// </summary>
        public string? humidity_ll { get; set;}

        /// <summary>
        /// 湿度上限
        /// </summary>
        public string? humidity_ul { get; set;}

        /// <summary>
        /// 温度
        /// </summary>
        public string? temperature { get; set;}

        /// <summary>
        /// 温度下限
        /// </summary>
        public string? temperature_ll { get; set;}

        /// <summary>
        /// 温度上限
        /// </summary>
        public string? temperature_ul { get; set;}



    }
}
