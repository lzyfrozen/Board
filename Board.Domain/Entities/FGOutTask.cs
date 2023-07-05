namespace Board.Domain.Entities
{
    /// <summary>
    /// 成品待出货任务
    /// </summary>
    public class FGOutTask
    {
        /// <summary>
        /// 物流类别
        /// </summary>
        public string? LogisticsCategory { get; set; } 

        /// <summary>
        /// 订单号
        /// </summary>
        public string? OrderNo { get; set; } 

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? Sku { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? SkuName { get; set; }

        /// <summary>
        /// 出货车牌
        /// </summary>
        public string? LicensePlate { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public string? StockQty { get; set; } 

        /// <summary>
        /// 卡板数量
        /// </summary>
        public string? PalletQty { get; set; } 

        /// <summary>
        /// 出货时间
        /// </summary>
        public DateTime? ShipmentTime { get; set; }

        /// <summary>
        /// 订货数量
        /// </summary>
        public string? QtyOrdered { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        public string? QtyShipped { get; set; }

        /// <summary>
        /// 客户Id(收货人)
        /// </summary>
        public string? ConsigneeId { get; set; }

        /// <summary>
        /// 客户名称(收货人)
        /// </summary>
        public string? ConsigneeName { get; set; } 

        /// <summary>
        /// 产品型号
        /// </summary>
        public string? Model { get; set; } = string.Empty;
    }
}