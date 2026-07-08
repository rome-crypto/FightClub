using AutoMapper;
using FightClub.Application.DTOs.Boxers;
using FightClub.Application.DTOs.Common;
using FightClub.Application.Interfaces;
using FightClub.Application.Specifications;
using FightClub.Domain.Entities;

namespace FightClub.Application.Services;

public class BoxerService 
    : BaseService<Boxer, BoxerResponseDto, BoxerCreateDto, BoxerUpdateDto>, IBoxerService
{
    public BoxerService(IRepository<Boxer> repo, IMapper mapper)
        : base(repo, mapper)
    {
    }

    protected override ISpecification<Boxer> CreateSpecification(BaseQueryDto query)
        => new BoxerSpecification((BoxerQueryDto)query);
}
