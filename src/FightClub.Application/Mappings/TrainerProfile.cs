using AutoMapper;
using FightClub.Application.DTOs.Trainers;
using FightClub.Domain.Entities;

namespace FightClub.Application.Mappings;

public class TrainerProfile : Profile
{
    public TrainerProfile()
    {
        CreateMap<Trainer, TrainerResponseDto>();

        CreateMap<TrainerCreateDto, Trainer>();

        CreateMap<TrainerUpdateDto, Trainer>();

        CreateMap<Trainer, TrainerWithBoxersDto>();
    }
}
