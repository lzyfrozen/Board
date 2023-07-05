using AutoMapper;
using Board.Domain.Entities;
using Board.Application.DataBoard.Dto;

namespace Board.AutoMappers.Profiles
{
    public class FGOutTaskProfile : Profile
    {
        public FGOutTaskProfile()
        {
            Configure();
        }

        private void Configure()
        {
            CreateMap<FGOutTask, FGOutTaskDto>()
                .ForMember(x=>x.订单号,opt=>opt.MapFrom(x=>x.OrderNo))
                .ForMember(x => x.物料编码, opt => opt.MapFrom(x => x.Sku))
                .ForMember(x => x.物料名称, opt => opt.MapFrom(x => x.SkuName))
                .ForMember(x => x.产品型号, opt => opt.MapFrom(x => x.Model))
                .ForMember(x => x.库存数量, opt => opt.MapFrom(x => x.StockQty))
                .ForMember(x => x.卡板数量, opt => opt.MapFrom(x => x.PalletQty))
                .ForMember(x => x.客户ID, opt => opt.MapFrom(x => x.ConsigneeId))
                .ForMember(x => x.客户名称, opt => opt.MapFrom(x => x.ConsigneeName))
                .ForMember(x => x.物流类别, opt => opt.MapFrom(x => x.LogisticsCategory))
                .ForMember(x => x.出货车牌, opt => opt.MapFrom(x => x.LicensePlate))
                .ForMember(x => x.订货数量, opt => opt.MapFrom(x => x.QtyOrdered))
                .ForMember(x => x.发货数量, opt => opt.MapFrom(x => x.QtyShipped))
                .ForMember(x => x.出货时间, opt => opt.MapFrom(x => x.ShipmentTime));
        }
    }
}
