using FightClub.Entities;
using AutoMapper;
using FightClub.DTOs;
namespace FightClub.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Boxer, BoxerResponseDto>();

        CreateMap<BoxerCreateDto, Boxer>();

        CreateMap<BoxerUpdateDto, Boxer>();
    }
}
