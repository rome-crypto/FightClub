using AutoMapper;
using AutoMapper.QueryableExtensions;
using FightClub.Domain.Common;
using FightClub.DTOs.Common;
using FightClub.Exceptions;
using FightClub.Repositories.Interfaces;
using FightClub.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FightClub.Services;

public abstract class BaseService<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : Entity
{
    protected readonly IRepository<TEntity> _repo;
    protected readonly IMapper _mapper;

    protected BaseService(IRepository<TEntity> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public virtual async Task<TDto> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");

        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto> CreateAsync(TCreateDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);

        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();

        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto> UpdateAsync(Guid id, TUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");

        _mapper.Map(dto, entity);

        await _repo.SaveChangesAsync();

        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");

        _repo.Delete(entity);
        await _repo.SaveChangesAsync();
    }

    public virtual async Task<PagedResult<TDto>> GetPagedAsync(BaseQueryDto query)
    {
        var spec = CreateSpecification(query);

        var total = await _repo.CountAsync(spec);

        var items = await _repo
            .Query(spec)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<TDto>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    protected abstract ISpecification<TEntity> CreateSpecification(BaseQueryDto query);
}
