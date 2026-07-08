using AutoMapper;
using FightClub.Application.DTOs.Fights.FightDetails;
using FightClub.Domain.Entities;

namespace FightClub.Application.Mappings;

public class FightProfile : Profile
{
    public FightProfile()
    {
        CreateMap<Fight, FightDetailsDto>();

        CreateMap<FightRound, RoundsDto>();

        CreateMap<RoundEvent, RoundEventDto>()
            .ForMember(d => d.Type,
                opt => opt.MapFrom(s => s.Type.ToString()));
    }
}
