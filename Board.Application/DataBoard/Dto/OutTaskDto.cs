namespace Board.Application.DataBoard.Dto
{
    /// <summary>
    /// 待出任务
    /// </summary>
    public class OutTaskDto
    {
        public long? 序号 { set; get; } 
        public string? 出库单号 { get; set; }
        public string? 事务类型 { get; set; }
        public DateTime? 下发时间 { get; set; }
    }
}
