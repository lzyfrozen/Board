using AutoMapper;
using Board.Domain.Entities;
using Board.Application.DataBoard.Dto;

namespace Board.AutoMappers.Profiles
{
    public class OutTaskProfile : Profile
    {
        public OutTaskProfile()
        {
            Configure();
        }

        private void Configure()
        {
            CreateMap<OutTask, OutTaskDto>()
                .ForMember(x => x.出库单号, opt => opt.MapFrom(x => x.OrderNo))
                .ForMember(x => x.事务类型, opt => opt.MapFrom(x => x.TransactionType))
                .ForMember(x => x.下发时间, opt => opt.MapFrom(x => x.AddTime));
        }
    }
}
