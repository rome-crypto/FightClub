using FightClub.Application.Exceptions;
using FightClub.Application.Interfaces;
using FightClub.Domain.Entities;
using FightClub.Domain.Enums;
using FightClub.Domain.Policies;
using FightClub.Domain.Services;
using FightClub.Domain.ValueObjects;


namespace FightClub.Application.Services;

/// <summary>
/// Сервис для обработки бизнес процесса боя
/// </summary>
/// <remarks>
/// Внедрение зависимостей
/// </remarks>
/// <param name="fightRepository">Репозиторий боя</param>
/// <param name="boxerRepository">Репозиторий боксеров</param>
/// <param name="roundSimulator">Симулятор раунда</param>
/// <param name="endingPolicy">Политика завершения</param>
/// <param name="resultService">Сервис для результата боя</param>
public sealed class FightSimulationService(
    IRepository<Fight> fightRepository,
    IRepository<Boxer> boxerRepository,
    IRoundSimulator roundSimulator,
    IFightEndingPolicy endingPolicy,
    IFightResultService resultService)
        : IFightSimulationService
{

    private readonly IRepository<Fight> _fightRepository = fightRepository;
    private readonly IRepository<Boxer> _boxerRepository = boxerRepository;

    private readonly IRoundSimulator _roundSimulator = roundSimulator;
    private readonly IFightEndingPolicy _endingPolicy = endingPolicy;

    private readonly IFightResultService _resultService = resultService;

    /// <summary>
    /// Отмена боя
    /// </summary>
    /// <param name="fightId">ID боя</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">Исключение поиска</exception>
    public async Task CancelAsync(Guid fightId)
    {
        Fight fight = await _fightRepository
            .GetByIdAsync(fightId)
            ?? throw new NotFoundException("Fight not found");

        fight.Cancel();
        await _fightRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Запуск боя
    /// </summary>
    /// <param name="fightId">ID боя</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">Исключение поиска</exception>
    public async Task ExecuteAsync(
        Guid fightId)
    {
        Fight fight = await _fightRepository
            .GetByIdAsync(fightId)
            ?? throw new NotFoundException("Fight not found");

        Boxer boxerA = await _boxerRepository
            .GetByIdAsync(fight.BoxerAId)
            ?? throw new NotFoundException("Boxer A not found");

        Boxer boxerB = await _boxerRepository
            .GetByIdAsync(fight.BoxerBId)
            ?? throw new NotFoundException("Boxer B not found");

        fight.Start();

        while (fight.Status != FightStatus.Finished)
        {
            fight.StartRound();

            RoundScore score = _roundSimulator
                .Simulate(
                fight.BoxerAId,
                    fight.BoxerBId);

            fight.EndCurrentRound(
                score,
                _endingPolicy);
        }

        _resultService.Apply(
            fight,
            boxerA,
            boxerB);

        await _fightRepository.SaveChangesAsync();
    }
}
