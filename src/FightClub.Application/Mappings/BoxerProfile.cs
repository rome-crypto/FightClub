using AutoMapper;
using FightClub.Application.DTOs.Boxers;
using FightClub.Domain.Entities;

namespace FightClub.Application.Mappings;


public class BoxerProfile : Profile
{
    public BoxerProfile()
    {
        CreateMap<Boxer, BoxerResponseDto>();
    }
}
