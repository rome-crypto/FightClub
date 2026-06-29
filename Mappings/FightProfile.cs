using AutoMapper;
using FightClub.DTOs.Fights;
using FightClub.Entities;
namespace FightClub.Mappings;

public class FightProfile : Profile
{
    public FightProfile()
    {
        CreateMap<Fight, FightResponseDto>()
            .ForMember(d => d.BoxerAName,
                o => o.MapFrom(s => s.BoxerA.FirstName + " " + s.BoxerA.LastName))
            .ForMember(d => d.BoxerBName,
                o => o.MapFrom(s => s.BoxerB.FirstName + " " + s.BoxerB.LastName))
            .ForMember(d => d.WinnerName,
                o => o.MapFrom(s =>
                    s.Winner == null
                        ? "Draw"
                        : s.Winner.FirstName + " " + s.Winner.LastName));
    }
}
