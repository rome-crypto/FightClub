namespace FightClub.Domain.ValueObjects;

public sealed class BoxerStatistics
{
    public int Wins { get; private set; }
    public int Losses { get; private set; }
    public int Draws { get; private set; }

    public int Knockouts { get; private set; }
    public int TechnicalKnockouts { get; private set; }

    public int KnockoutLosses { get; private set; }
    public int TechnicalKnockoutLosses { get; private set; }

    public int WinStreak { get; private set; }
    public int BestWinStreak { get; private set; }

    public DateTime? LastFightDate { get; private set; }

    public int TotalFights => Wins + Losses + Draws;

    public double WinRate =>
        TotalFights == 0
        ? 0
        : Math.Round((double)Wins / (TotalFights) * 100, 2);

    public double KnockoutRate =>
        Wins == 0
        ? 0
        : Math.Round((double)Knockouts / (Wins) * 100, 2);

    internal BoxerStatistics() { }

    internal void RegisterWin(bool knockout = false, bool technicalKnockout = false) 
    {
        Wins++; 
        WinStreak++;

        if (WinStreak > BestWinStreak)
            BestWinStreak = WinStreak;
        if (knockout)
            Knockouts++;
        if (technicalKnockout)
            TechnicalKnockouts++;

        LastFightDate = DateTime.UtcNow;
    }
    internal void RegisterLoss(bool knockout = false, bool technicalKnockout = false) 
    {
        Losses++; 
        WinStreak = 0;

        if (knockout)
            KnockoutLosses++;
        if (technicalKnockout)
            TechnicalKnockoutLosses++;

        LastFightDate = DateTime.UtcNow;
    }
    internal void RegisterDraw()
    {
        Draws++;
        WinStreak = 0;
        LastFightDate = DateTime.UtcNow;
    }
}
