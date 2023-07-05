using AutoMapper;
using Board.Domain.Entities;
using Board.Application.DataBoard.Dto;

namespace Board.AutoMappers.Profiles
{
    public class HumitureProfile : Profile
    {
        public HumitureProfile()
        {
            Configure();
        }

        private void Configure()
        {
            CreateMap<Humiture, HumitureDto>()
                .ForMember(x => x.温湿度仪名称, opt => opt.MapFrom(x => x.deviceId))
                .ForMember(x => x.湿度, opt => opt.MapFrom(x => x.humidity))
                .ForMember(x => x.湿度下限, opt => opt.MapFrom(x => x.humidity_ll))
                .ForMember(x => x.湿度上限, opt => opt.MapFrom(x => x.humidity_ul))
                .ForMember(x => x.温度, opt => opt.MapFrom(x => x.temperature))
                .ForMember(x => x.温度下限, opt => opt.MapFrom(x => x.temperature_ll))
                .ForMember(x => x.温度上限, opt => opt.MapFrom(x => x.temperature_ul));
        }
    }
}
