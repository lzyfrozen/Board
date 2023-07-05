using AutoMapper;
using Board.Domain.Entities;
using Board.Application.DataBoard.Dto;

namespace Board.AutoMappers.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            Configure();
        }

        private void Configure()
        {
            CreateMap<Location, LocationDto>()
                .ForMember(x => x.库位总数, opt => opt.MapFrom(x => x.TotalBin))
                .ForMember(x => x.使用数量, opt => opt.MapFrom(x => x.UseBin))
                .ForMember(x => x.空仓, opt => opt.MapFrom(x => x.FreelBin))
                .ForMember(x => x.库存超期预警, opt => opt.MapFrom(x => x.ExprieAlert));
        }
    }
}
