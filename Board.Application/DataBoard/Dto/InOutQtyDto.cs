namespace Board.Application.DataBoard.Dto
{
    /// <summary>
    /// 出入库数量
    /// </summary>
    public class InOutQtyDto
    {
        public string? 日期 { get; set; }
        public string? 天数 { get; set; }
        public decimal? 数量 { get; set; }
        public decimal? 入库数量 { get; set; }
        public decimal? 出库数量 { get; set; }

    }
}
