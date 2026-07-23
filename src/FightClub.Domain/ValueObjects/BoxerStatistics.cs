using FightClub.Domain.Common;

namespace FightClub.Domain.ValueObjects;

public sealed class BoxerStatistics : ValueObject
{
    public int Wins { get; }
    public int Losses { get; }
    public int Draws { get; }

    public int Knockouts { get; }
    public int TechnicalKnockouts { get; }

    public int KnockoutLosses { get; }
    public int TechnicalKnockoutLosses { get; }

    public int WinStreak { get; }
    public int BestWinStreak { get; }

    public DateTime? LastFightDate { get; }

    public int TotalFights => Wins + Losses + Draws;

    public double WinRate =>
        TotalFights == 0
        ? 0
        : Math.Round((double)Wins / TotalFights * 100, 2);

    public double KnockoutRate =>
        Wins == 0
        ? 0
        : Math.Round((double)Knockouts / Wins * 100, 2);

    internal BoxerStatistics()
    {
    }

    private BoxerStatistics(
        int wins,
        int losses,
        int draws,
        int knockouts,
        int technicalKnockouts,
        int knockoutLosses,
        int technicalKnockoutLosses,
        int winStreak,
        int bestWinStreak,
        DateTime? lastFightDate)
    {
        Wins = wins;
        Losses = losses;
        Draws = draws;
        Knockouts = knockouts;
        TechnicalKnockouts = technicalKnockouts;
        KnockoutLosses = knockoutLosses;
        TechnicalKnockoutLosses = technicalKnockoutLosses;
        WinStreak = winStreak;
        BestWinStreak = bestWinStreak;
        LastFightDate = lastFightDate;
    }

    internal BoxerStatistics RegisterWin(bool knockout = false, bool technicalKnockout = false)
    {
        var newWinStreak = WinStreak + 1;

        return new BoxerStatistics(
            wins: Wins + 1,
            losses: Losses,
            draws: Draws,
            knockouts: Knockouts + (knockout ? 1 : 0),
            technicalKnockouts: TechnicalKnockouts + (technicalKnockout ? 1 : 0),
            knockoutLosses: KnockoutLosses,
            technicalKnockoutLosses: TechnicalKnockoutLosses,
            winStreak: newWinStreak,
            bestWinStreak: Math.Max(BestWinStreak, newWinStreak),
            lastFightDate: DateTime.UtcNow
        );
    }

    internal BoxerStatistics RegisterLoss(bool knockout = false, bool technicalKnockout = false)
    {
        return new BoxerStatistics(
            wins: Wins,
            losses: Losses + 1,
            draws: Draws,
            knockouts: Knockouts,
            technicalKnockouts: TechnicalKnockouts,
            knockoutLosses: KnockoutLosses + (knockout ? 1 : 0),
            technicalKnockoutLosses: TechnicalKnockoutLosses + (technicalKnockout ? 1 : 0),
            winStreak: 0,
            bestWinStreak: BestWinStreak,
            lastFightDate: DateTime.UtcNow
        );
    }

    internal BoxerStatistics RegisterDraw()
    {
        return new BoxerStatistics(
            wins: Wins,
            losses: Losses,
            draws: Draws + 1,
            knockouts: Knockouts,
            technicalKnockouts: TechnicalKnockouts,
            knockoutLosses: KnockoutLosses,
            technicalKnockoutLosses: TechnicalKnockoutLosses,
            winStreak: 0,
            bestWinStreak: BestWinStreak,
            lastFightDate: DateTime.UtcNow
        );
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Wins;
        yield return Losses;
        yield return Draws;
        yield return Knockouts;
        yield return TechnicalKnockouts;
        yield return KnockoutLosses;
        yield return TechnicalKnockoutLosses;
        yield return WinStreak;
        yield return BestWinStreak;
        yield return LastFightDate;
    }
}
