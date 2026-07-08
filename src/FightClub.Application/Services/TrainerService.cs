using AutoMapper;
using FightClub.Application.DTOs.Common;
using FightClub.Application.DTOs.Trainers;
using FightClub.Application.Interfaces;
using FightClub.Application.Specifications;
using FightClub.Domain.Entities;

namespace FightClub.Application.Services;

public class TrainerService 
    : BaseService<Trainer, TrainerResponseDto, TrainerCreateDto, TrainerUpdateDto>, ITrainerService
{

    public TrainerService(IRepository<Trainer> repo, IMapper mapper)
        : base(repo, mapper)
    {
    }

    protected override ISpecification<Trainer> CreateSpecification(BaseQueryDto query)
        => new TrainerSpecification((TrainerQueryDto)query);
}
