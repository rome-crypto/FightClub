using AutoMapper;
using FightClub.DTOs.Common;
using FightClub.DTOs.Fights;
using FightClub.DTOs.Trainers;
using FightClub.Entities;
using FightClub.Exceptions;
using FightClub.Repositories.Interfaces;
using FightClub.Services.Interfaces;
using FightClub.Specifications;

namespace FightClub.Services;

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
