using AutoMapper;
using AutoMapper.QueryableExtensions;
using FightClub.DTOs.Boxers;
using FightClub.DTOs.Common;
using FightClub.DTOs.Trainers;
using FightClub.Entities;
using FightClub.Exceptions;
using FightClub.Repositories.Interfaces;
using FightClub.Services.Interfaces;
using FightClub.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Services;

public class BoxerService 
    : BaseService<Boxer, BoxerResponseDto, BoxerCreateDto, BoxerUpdateDto>, IBoxerService
{
    public BoxerService(IBoxerRepository repo, IMapper mapper)
        : base(repo, mapper)
    {
    }

    protected override ISpecification<Boxer> CreateSpecification(BaseQueryDto query)
        => new BoxerSpecification((BoxerQueryDto)query);
}
