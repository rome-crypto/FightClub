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
                dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(
                dest => dest.BoxersCount,
                opt => opt.MapFrom(src => src.Boxers.Count));

        CreateMap<TrainerCreateDto, Trainer>();

        CreateMap<TrainerUpdateDto, Trainer>();

        CreateMap<Trainer, TrainerWithBoxersDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
