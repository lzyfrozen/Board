namespace Board.Domain.Entities
{
    /// <summary>
    /// 出入库数量
    /// </summary>
    public class InOutQty
    {
        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; } 

        /// <summary>
        /// 出入类型
        /// </summary>
        public string? InOutType { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string? Dates { get; set; }

        /// <summary>
        /// 天数
        /// </summary>
        public string? Days { get; set; }
    }
}
