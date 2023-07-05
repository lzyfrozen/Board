using AutoMapper;
using Board.Domain.Entities;
using Board.Application.DataBoard.Dto;

namespace Board.AutoMappers.Profiles
{
    public class InOutQtyProfile : Profile
    {
        public InOutQtyProfile()
        {
            Configure();
        }

        private void Configure()
        {
            CreateMap<InOutQty, InOutQtyDto>()
                .ForMember(x => x.日期, opt => opt.MapFrom(x => x.Dates))
                .ForMember(x => x.天数, opt => opt.MapFrom(x => x.Days))
                .ForMember(x => x.数量, opt => opt.MapFrom(x => x.Qty));
        }
    }
}
