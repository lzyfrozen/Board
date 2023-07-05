namespace Board.Application.DataBoard.Dto
{
    /// <summary>
    /// 待入任务
    /// </summary>
    public class InTaskDto
    {
        public long? 序号 { set; get; }
        public string? 入库单号 { get; set; }
        public string? 事务类型 { get; set; }
        public DateTime? 下发时间 { get; set; } 
 
    }
}
