namespace Board.Application.DataBoard.Dto
{
    /// <summary>
    /// 成品待出货任务
    /// </summary>
    public class FGOutTaskDto
    {
        public long? 序号 { get; set; }
        public string? 物流类别 { get; set; }
        public string? 订单号 { get; set; }
        public string? 物料编码 { get; set; }
        public string? 物料名称 { get; set; }
        public string? 出货车牌 { get; set; }
        public string? 库存数量 { get; set; }
        public string? 卡板数量 { get; set; }
        public DateTime? 出货时间 { get; set; }
        public string? 出货日期 { get { return 出货时间?.ToString("yyyy-MM-dd"); } }
        public string? 订货数量 { get; set; }
        public string? 发货数量 { get; set; }
        public string? 客户ID { get; set; }
        public string? 客户名称 { get; set; }
        public string? 产品型号 { get; set; }
    }
}