using FightClub.Entities;
using AutoMapper;
using FightClub.DTOs.Boxers;
namespace FightClub.Mappings;


public class BoxerProfile : Profile
{
    public BoxerProfile()
    {
        CreateMap<Boxer, BoxerResponseDto>();

        CreateMap<BoxerCreateDto, Boxer>();

        CreateMap<BoxerUpdateDto, Boxer>();

        //CreateMap<Boxer, BoxerDetailsDto>();
    }
}
