using AutoMapper;
using FightClub.Application.DTOs.Fights;
using FightClub.Application.DTOs.Fights.FightDetails;
using FightClub.Domain.Entities;

namespace FightClub.Application.Mappings;

public sealed class FightProfile : Profile
{
    public FightProfile()
    {
        CreateMap<Fight, FightResponseDto>();

        CreateMap<FightRound, RoundsDto>();

        CreateMap<RoundEvent, RoundEventDto>();
    }
}