using AutoMapper;
using FightClub.DTOs.Fights;
using FightClub.DTOs.Fights.FightDetails;
using FightClub.Entities.Fight;
namespace FightClub.Mappings;

public class FightProfile : Profile
{
    public FightProfile()
    {
        CreateMap<Fight, FightDetailsDto>()
            .ForMember(d => d.BoxerAName,
                opt => opt.MapFrom(s => s.BoxerA.FirstName + " " + s.BoxerA.LastName))
            .ForMember(d => d.BoxerBName,
                opt => opt.MapFrom(s => s.BoxerB.FirstName + " " + s.BoxerB.LastName));

        CreateMap<FightRound, RoundsDto>();

        CreateMap<RoundEvent, RoundEventDto>()
            .ForMember(d => d.Type,
                opt => opt.MapFrom(s => s.Type.ToString()));
    }
}
