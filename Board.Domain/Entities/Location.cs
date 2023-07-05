namespace Board.Domain.Entities
{
    /// <summary>
    /// 库位信息
    /// </summary>
    public class Location
    {
        /// <summary>
        /// 库位总数
        /// </summary>
        public long TotalBin { get; set; }
        
        /// <summary>
        /// 库位使用数
        /// </summary>
        public long UseBin { get; set; }

        /// <summary>
        /// 空闲库位数
        /// </summary>
        public long FreelBin { get; set; }

        /// <summary>
        /// 库存超期预警
        /// </summary>
        public long ExprieAlert { get; set; }



    }
}
