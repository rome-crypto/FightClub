using FightClub.Domain.Common;
using FightClub.Domain.Enums;

namespace FightClub.Domain.ValueObjects;

public sealed class WeightCategory : ValueObject
{
    public string Name { get; }
    public double MinWeight { get; }
    public double MaxWeight { get; }
    public WeightCategoryType Type => Name switch
    {
        "Heavyweight" => WeightCategoryType.Heavyweight,
        "Middleweight" => WeightCategoryType.Middleweight,
        "Lightweight" => WeightCategoryType.Lightweight,
        _ => throw new InvalidOperationException($"Unknown weight category: {Name}")
    };

    private WeightCategory(string name, double min, double max)
    {
        Name = name;
        MinWeight = min;
        MaxWeight = max;
    }

    public static WeightCategory Heavyweight => new("Heavyweight", 90.0, 120.0);
    public static WeightCategory Middleweight => new("Middleweight", 70.0, 90.0);
    public static WeightCategory Lightweight => new("Lightweight", 0, 70.0);

    public static WeightCategory FromWeight(double weight)
    {
        return weight switch
        {
            >= 90 => Heavyweight,
            >= 70 => Middleweight,
            _ => Lightweight
        };
    }

    public override string ToString()
    {
        return Name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return MinWeight;
        yield return MaxWeight;
        yield return Type;
    }
}
