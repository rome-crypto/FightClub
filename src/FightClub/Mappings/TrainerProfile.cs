using AutoMapper;
using FightClub.DTOs.Trainers;
using FightClub.Entities;

namespace FightClub.Mappings;

public class TrainerProfile : Profile
{
    public TrainerProfile()
    {
        CreateMap<Trainer, TrainerResponseDto>()
            .ForMember(
                dest => dest.BoxersCount,
                opt => opt.MapFrom(src => src.Boxers.Count));

        CreateMap<TrainerCreateDto, Trainer>();

        CreateMap<TrainerUpdateDto, Trainer>();

        CreateMap<Trainer, TrainerWithBoxersDto>();
    }
}
