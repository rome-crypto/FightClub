using FightClub.Domain.Common;

namespace FightClub.Domain.ValueObjects;

public sealed class WeightCategory :ValueObject
{
    public string Name { get; }
    public double MinWeight { get; }
    public double MaxWeight { get; }

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

    public override string ToString() => Name;
}
