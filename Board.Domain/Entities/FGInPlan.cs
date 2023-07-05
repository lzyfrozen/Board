namespace Board.Domain.Entities
{
    public class FGInPlan
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string? Sku { get; set; } 
        /// <summary>
        /// 产品型号
        /// </summary>
        public string? ProductModel { get; set; } 

        /// <summary>
        /// 计划
        /// </summary>
        public string? PlanQty { get; set; } 

        /// <summary>
        /// 完成
        /// </summary>
        public string? CompleteQty { get; set; } 
    }
}
