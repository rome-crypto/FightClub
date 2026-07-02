using AutoMapper;
using FightClub.DTOs.Fights;
using FightClub.Entities.Fight;
namespace FightClub.Mappings;

public class FightProfile : Profile
{
    public FightProfile()
    {
        CreateMap<Fight, FightResponseDto>();
        CreateMap<FightRound, FightRoundDto>();
        CreateMap<RoundEvent, RoundEventDto>();
    }
}
