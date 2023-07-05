using AutoMapper;
using Board.Domain.Entities;
using Board.Application.DataBoard.Dto;

namespace Board.AutoMappers.Profiles
{
    public class FGInPlanProfile : Profile
    {
        public FGInPlanProfile()
        {
            Configure();
        }

        private void Configure()
        {
            CreateMap<FGInPlan, FGInPlanDto>()
                .ForMember(x => x.物料编码, opt => opt.MapFrom(x => x.Sku))
                .ForMember(x => x.产品型号, opt => opt.MapFrom(x => x.ProductModel))
                .ForMember(x => x.计划, opt => opt.MapFrom(x => x.PlanQty))
                .ForMember(x => x.完成, opt => opt.MapFrom(x => x.CompleteQty));
        }
    }
}
