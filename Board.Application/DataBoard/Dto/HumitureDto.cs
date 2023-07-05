namespace Board.Application.DataBoard.Dto
{
    /// <summary>
    /// 温湿度
    /// </summary>
    public class HumitureDto
    {
        public string? 温湿度仪名称 { get; set; }

        public string? 连接状态 { get; set; }

        public string? 湿度预警 { get; set; }

        public string? 温度预警 { get; set; }

        public string? 湿度 { get; set; }

        public string? 湿度下限 { get; set; }

        public string? 湿度上限 { get; set; }

        public string? 温度 { get; set; }

        public string? 温度下限 { get; set; }

        public string? 温度上限 { get; set; }
    }
}
