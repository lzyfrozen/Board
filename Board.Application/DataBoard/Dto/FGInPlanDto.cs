namespace Board.Application.DataBoard.Dto
{
    /// <summary>
    /// 成品入库计划达成进度
    /// </summary>
    public class FGInPlanDto
    {
        public string? 物料编码 { get; set; }
        public string? 产品型号 { get; set; }
        public string? 计划 { get; set; }
        public string? 完成 { get; set; }
    }
}
