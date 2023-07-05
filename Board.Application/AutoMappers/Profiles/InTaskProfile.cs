using AutoMapper;
using Board.Domain.Entities;
using Board.Application.DataBoard.Dto;

namespace Board.AutoMappers.Profiles
{
    public class InTaskProfile : Profile
    {
        public InTaskProfile()
        {
            Configure();
        }

        private void Configure()
        {
            CreateMap<InTask, InTaskDto>()
                .ForMember(x => x.入库单号, opt => opt.MapFrom(x => x.AsnNo))
                .ForMember(x => x.事务类型, opt => opt.MapFrom(x => x.TransactionType))
                .ForMember(x => x.下发时间, opt => opt.MapFrom(x => x.AddTime));
        }
    }
}
