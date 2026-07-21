using FightClub.Domain.Common;
using FightClub.Domain.Enums;
using FightClub.Domain.Exceptions;
using FightClub.Domain.Policies;
using FightClub.Domain.ValueObjects;

namespace FightClub.Domain.Entities;

/// <summary>
/// Доменная сущность боя
/// </summary>
/// <remarks>Является AggregateRoot для FightRound, RoundEvent</remarks>
public class Fight : AggregateRoot
{
    /// <summary>
    /// Максимальное количество запланнированных раундов
    /// </summary>
    public const int MaxPlannedRounds = 24;

    /// <summary>
    /// Лист раундов
    /// </summary>
    private readonly List<FightRound> _rounds = [];

    /// <summary>
    /// ID боксера А
    /// </summary>
    public Guid BoxerAId { get; private set; }
    /// <summary>
    /// ID боксера Б
    /// </summary>
    public Guid BoxerBId { get; private set; }

    /// <summary>
    /// ID победителя
    /// </summary>
    /// <remarks>null при ничьей</remarks>
    public Guid? WinnerId { get; private set; }
    /// <summary>
    /// Статус боя
    /// </summary>
    public FightStatus Status { get; private set; }
    /// <summary>
    /// Причина завершения
    /// </summary>
    /// <remarks>null если бой еще не окончен</remarks>
    public FightEndType? EndType { get; private set; }

    /// <summary>
    /// Запланированно раундов
    /// </summary>
    /// <remarks>По умолчанию: 12</remarks>
    public int PlannedRounds { get; private set; }

    /// <summary>
    /// Текущее количество раундов
    /// </summary>
    /// <remarks>Вычислимое</remarks>
    public int ActualRounds => _rounds.Count;

    /// <summary>
    /// Ссылка на лист раундов
    /// </summary>
    public IReadOnlyCollection<FightRound> Rounds => _rounds;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Запланированная дата боя
    /// </summary>
    public DateTime? FightDate { get; private set; }

    /// <summary>
    /// Разрешение на изменения
    /// </summary>
    public bool IsAllowedChanges => Status == FightStatus.Scheduled || Status == FightStatus.Created;

    private Fight() { }

    /// <summary>
    /// Конструктор-фабрика
    /// </summary>
    /// <param name="boxerAId">ID боксера А</param>
    /// <param name="boxerBId">ID боксера Б</param>
    /// <param name="plannedRounds">Запланированное количество раундов</param>
    /// <param name="fightDate">Запланированное время боя</param>
    public Fight(Guid boxerAId, Guid boxerBId, DateTime? fightDate = null, int plannedRounds = 12)
    {
        if (boxerAId == boxerBId)
        {
            throw new DomainException("Boxer cannot fight himself");
        }
        if (plannedRounds < 1 || plannedRounds > MaxPlannedRounds)
        {
            throw new DomainException($"Rounds must be between 1 and {MaxPlannedRounds}");
        }

        BoxerAId = boxerAId;
        BoxerBId = boxerBId;
        PlannedRounds = plannedRounds;
        CreatedAt = DateTime.UtcNow;
        Reschedule(fightDate);
    }

    /// <summary>
    /// Проверка возможности удаления
    /// </summary>
    /// <exception cref="DomainException">Исключение доменной логики</exception>
    public void EnsureCanBeDeleted()
    {
        if (Status == FightStatus.InProgress)
        {
            throw new DomainException("Cannot delete fight while it is in progress");
        }

        if (Status == FightStatus.Finished)
        {
            throw new DomainException("Cannot delete finished fights");
        }
    }

    /// <summary>
    /// Запланировать дату боя
    /// </summary>
    /// <param name="fightDate">Дата боя</param>
    /// <exception cref="DomainException"></exception>
    public void Reschedule(DateTime? fightDate)
    {
        if (Status != FightStatus.Created && Status != FightStatus.Scheduled)
        {
            throw new DomainException("Fight cannot be rescheduled");
        }

        if (fightDate == null)
        {
            FightDate = null;
            Status = FightStatus.Created;
            return;
        }

        if (fightDate <= DateTime.UtcNow)
        {
            throw new DomainException("Fight date must be in the future");
        }

        FightDate = fightDate;
        Status = FightStatus.Scheduled;
    }

    /// <summary>
    /// Запуск боя
    /// </summary>
    public void Start()
    {
        if (Status != FightStatus.Scheduled)
        {
            throw new DomainException("Fight cannot started");
        }

        Status = FightStatus.InProgress;
    }

    /// <summary>
    /// Начало раунда
    /// </summary>
    public void StartRound()
    {
        if (_rounds.LastOrDefault() is { IsFinished: false })
        {
            throw new DomainException("Finish current round first");
        }

        if (Status != FightStatus.InProgress)
        {
            throw new DomainException("Fight not active");
        }


        if (ActualRounds >= PlannedRounds)
        {
            throw new DomainException("Maximum rounds reached");
        }

        _rounds.Add(
            new FightRound(ActualRounds + 1)
        );
    }

    /// <summary>
    /// Регистрация события
    /// </summary>
    /// <param name="roundEvent">событие</param>
    public void RegisterEvent(RoundEvent roundEvent)
    {
        if (Status != FightStatus.InProgress)
        {
            throw new DomainException("Fight not active");
        }

        if (ActualRounds == 0 || _rounds.Last().IsFinished)
        {
            throw new DomainException("No active round");
        }

        if (roundEvent.BoxerId != BoxerAId &&
            roundEvent.BoxerId != BoxerBId)
        {
            throw new DomainException("Boxer is not a participant of this fight.");
        }

        _rounds.Last().AddEvent(roundEvent);
    }

    /// <summary>
    /// Закончить текущий раунд
    /// </summary>
    /// <param name="score">счет раунда</param>
    /// <param name="endingPolicy">политика завершения</param>
    public void EndCurrentRound(
        RoundScore score,
        IFightEndingPolicy endingPolicy)
    {
        if (Status != FightStatus.InProgress)
        {
            throw new DomainException("Fight not active");
        }

        if (_rounds.Count == 0)
        {
            throw new DomainException("No active round");
        }

        FightRound round = _rounds.Last();

        round.SetScore(score);

        FightOutcome outcome = endingPolicy.Evaluate(
            _rounds,
            BoxerAId,
            BoxerBId,
            PlannedRounds);

        if (outcome.IsFinished)
        {
            Complete(outcome);
        }
    }

    /// <summary>
    /// Закончить бой
    /// </summary>
    /// <param name="outcome">результаты</param>
    public void Complete(FightOutcome outcome)
    {
        if (Status != FightStatus.InProgress)
        {
            throw new DomainException("Fight must be in progress");
        }

        if (!outcome.IsFinished)
        {
            throw new DomainException("Fight outcome is not finished");
        }

        WinnerId = outcome.WinnerId;
        EndType = outcome.EndType;
        Status = FightStatus.Finished;
    }

    /// <summary>
    /// Отменить бой
    /// </summary>
    public void Cancel()
    {
        if (Status != FightStatus.Scheduled)
        {
            throw new DomainException("Fight cannot to cancel");
        }

        Status = FightStatus.Cancelled;
    }
}
