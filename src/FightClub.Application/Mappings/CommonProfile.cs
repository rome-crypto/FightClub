using AutoMapper;
using FightClub.DTOs.Common;

namespace FightClub.Mappings;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
    }
}
