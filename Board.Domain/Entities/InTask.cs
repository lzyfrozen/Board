namespace Board.Domain.Entities
{
    public class InTask
    {
        /// <summary>
        /// 入库单号
        /// </summary>
        public string? AsnNo { get; set; }


        /// <summary>
        /// 事务类型
        /// </summary>
        public string? TransactionType { get; set; }

        /// <summary>
        /// 下发时间
        /// </summary>
        public DateTime? AddTime { get; set; }
    }
}
