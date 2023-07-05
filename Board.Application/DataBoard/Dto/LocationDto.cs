namespace Board.Application.DataBoard.Dto
{
    /// <summary>
    /// 库位信息
    /// </summary>
    public class LocationDto
    {
        public long 库位总数 { get; set; }

        public long 使用数量 { get; set; }

        public long 空仓 { get; set; }

        public long 库存超期预警 { get; set; }



    }
}
