using AutoMapper;
using FightClub.Application.DTOs.Boxers;
using FightClub.Domain.Entities;

namespace FightClub.Application.Mappings;

public class BoxerProfile : Profile
{
    public BoxerProfile()
    {
        CreateMap<Boxer, BoxerResponseDto>()
            .ForMember(d => d.FullName,
                o => o.MapFrom(s => s.FullName))
            .ForMember(d => d.WeightCategory,
                o => o.MapFrom(s => s.WeightCategory.Name))
            .ForMember(d => d.EloRating,
                o => o.MapFrom(s => s.Ranking.EloRating))
            .ForMember(d => d.Wins,
                o => o.MapFrom(s => s.Statistics.Wins))
            .ForMember(d => d.Losses,
                o => o.MapFrom(s => s.Statistics.Losses))
            .ForMember(d => d.Draws,
                o => o.MapFrom(s => s.Statistics.Draws))
            .ForMember(d => d.WinRate,
                o => o.MapFrom(s => s.Statistics.WinRate));

        CreateMap<BoxerCreateDto, Boxer>()
            .ConstructUsing(dto =>
                new Boxer(
                    dto.FirstName,
                    dto.LastName,
                    dto.BirthDate,
                    dto.Weight,
                    dto.TrainerId));
    }
}
