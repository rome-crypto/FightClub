using FightClub.DTOs.Boxers;
using FightClub.Entities;

namespace FightClub.Mappers;

public class BoxerMapper
{
    public static BoxerResponseDto ToDto(Boxer boxer)
    {
        return new BoxerResponseDto
        {
            Id = boxer.Id,
            FirstName = boxer.FirstName,
            LastName = boxer.LastName,
            Age = boxer.Age,
            WeightCategory = boxer.WeightCategory
        };
    }
}
