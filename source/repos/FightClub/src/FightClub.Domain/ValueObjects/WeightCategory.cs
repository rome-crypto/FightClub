using FightClub.Domain.Common;
using FightClub.Domain.Enums;

namespace FightClub.Domain.ValueObjects;

public sealed class WeightCategory : ValueObject
{
    public WeightCategoryType Type { get; }
    public double MinWeight { get; }
    public double MaxWeight { get; }
    public string Name => Type.ToString();

    private WeightCategory(WeightCategoryType type, double min, double max)
    {
        Type = type;
        MinWeight = min;
        MaxWeight = max;
    }

    public static WeightCategory Heavyweight => new(WeightCategoryType.Heavyweight, 90.0, 120.0);
    public static WeightCategory Middleweight => new(WeightCategoryType.Middleweight, 70.0, 90.0);
    public static WeightCategory Lightweight => new(WeightCategoryType.Lightweight, 50.0, 70.0);

    public static WeightCategory FromWeight(double weight)
    {
        return weight switch
        {
            >= 90 => Heavyweight,
            >= 70 => Middleweight,
            >= 50 => Lightweight,
            _ => throw new ArgumentException($"Weight {weight} is below minimum boxing weight (50kg)")
        };
    }

    public override string ToString()
    {
        return Name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return MinWeight;
        yield return MaxWeight;
    }
}
