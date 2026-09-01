using FightClub.Domain.Entities;
using FightClub.Domain.Enums;
using FightClub.Domain.Exceptions;
using FightClub.Domain.ValueObjects;

namespace FightClub.Domain.Tests.Entities;

[TestClass]
public sealed class BoxerTests
{
    [TestMethod]
    public void CreateValidBoxerWithValidParameters()
    {
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);

        Assert.IsNotNull(boxer);
        Assert.AreEqual("John", boxer.FirstName);
        Assert.AreEqual("Doe", boxer.LastName);
        Assert.AreEqual(100, boxer.Weight);
        Assert.AreEqual(DateTime.UtcNow.AddYears(-25).Date, boxer.DateOfBirth.Date);
        Assert.AreNotEqual(Guid.Empty, boxer.Id);
        Assert.IsNotNull(boxer.Statistics);
        Assert.IsNotNull(boxer.Ranking);
        Assert.IsNull(boxer.TrainerId);
        Assert.AreEqual(WeightCategory.Heavyweight, boxer.WeightCategory);
    }

    [TestMethod]
    [DataRow(17, true)] // Меньше 18
    [DataRow(18, false)] // Ровно 18
    [DataRow(80, false)] // Ровно 80
    [DataRow(81, true)] // Больше 80
    public void CreateBoxerShouldValidateAge(int ageOffset, bool shouldThrow)
    {
        // Arrange
        DateTime birthDate = DateTime.UtcNow.AddYears(-ageOffset);

        // Act & Assert
        if (shouldThrow)
        {
            Assert.ThrowsExactly<DomainException>(() =>
                new Boxer("John", "Doe", birthDate, 100));
        }
        else
        {
            var boxer = new Boxer("John", "Doe", birthDate, 100);
            Assert.IsNotNull(boxer);
        }
    }

    [TestMethod]
    public void RenameBoxerWithValidNamesShouldUpdateNames()
    {
        // Arrange
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
        const string newFirstName = "Jane";
        const string newLastName = "Smith";

        // Act
        boxer.Rename(newFirstName, newLastName);

        // Assert
        Assert.AreEqual(newFirstName, boxer.FirstName);
        Assert.AreEqual(newLastName, boxer.LastName);
        Assert.AreEqual("Jane Smith", boxer.FullName);
    }

    [TestMethod]
    public void RenameBoxerWithNullNamesShouldNotChange()
    {
        // Arrange
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
        var originalFirstName = boxer.FirstName;
        var originalLastName = boxer.LastName;

        // Act
        boxer.Rename(null, null);

        // Assert
        Assert.AreEqual(originalFirstName, boxer.FirstName);
        Assert.AreEqual(originalLastName, boxer.LastName);
    }

    [TestMethod]
    public void RenameBoxerWithEmptyNamesShouldThrow()
    {
        // Arrange
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);

        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() =>
            boxer.Rename("", "Smith"));
        Assert.ThrowsExactly<DomainException>(() =>
            boxer.Rename("John", ""));
        Assert.ThrowsExactly<DomainException>(() =>
            boxer.Rename("John", new string('a', 101)));
    }

    [TestMethod]
    public void ChangeWeightWithValidWeightShouldUpdateCategory()
    {
        // Arrange
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
        const int NewWeight = 75;

        // Act
        boxer.ChangeWeight(NewWeight);

        // Assert
        Assert.AreEqual(NewWeight, boxer.Weight);
        Assert.AreEqual(WeightCategory.Middleweight, boxer.WeightCategory);
    }

    [TestMethod]
    [DataRow(29, true)] // Слишком легкий
    [DataRow(201, true)] // Слишком тяжелый
    [DataRow(30, false)] // Минимум
    [DataRow(200, false)] // Максимум
    public void ChangeWeightShouldValidateWeightRange(int weight, bool shouldThrow)
    {
        // Arrange
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);

        // Act & Assert
        if (shouldThrow)
        {
            Assert.ThrowsExactly<DomainException>(() =>
                boxer.ChangeWeight(weight));
        }
        else
        {
            boxer.ChangeWeight(weight);
            Assert.AreEqual(weight, boxer.Weight);
        }
    }

    [TestMethod]
    public void ChangeBirthDateWithValidDateShouldUpdateAge()
    {
        // Arrange
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
        DateTime newBirthDate = DateTime.UtcNow.AddYears(-30);

        // Act
        boxer.ChangeBirthDate(newBirthDate);

        // Assert
        Assert.AreEqual(newBirthDate.Date, boxer.DateOfBirth.Date);
        Assert.AreEqual(30, boxer.Age);
    }

    [TestMethod]
    public void AssignTrainerShouldUpdateTrainerId()
    {
        // Arrange
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
        var trainerId = Guid.NewGuid();

        // Act
        boxer.AssignTrainer(trainerId);

        // Assert
        Assert.AreEqual(trainerId, boxer.TrainerId);
    }

    [TestMethod]
    public void AssignTrainerWithNullShouldRemoveTrainer()
    {
        // Arrange
        var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
        boxer.AssignTrainer(Guid.NewGuid());

        // Act
        boxer.AssignTrainer(null);

        // Assert
        Assert.IsNull(boxer.TrainerId);
    }

    //[TestMethod]
    //public void ApplyFightResultWinShouldUpdateStatisticsAndRanking()
    //{
    //    // Arrange
    //    var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
    //    const int ExpectedRating = 1532;

    //    // Act
    //    boxer.ApplyFightResult(FightResult.Win, FightEndType.Knockout, ExpectedRating);

    //    // Assert
    //    Assert.AreEqual(1, boxer.Statistics.Wins);
    //    Assert.AreEqual(0, boxer.Statistics.Losses);
    //    Assert.AreEqual(0, boxer.Statistics.Draws);
    //    Assert.AreEqual(1, boxer.Statistics.TotalFights);
    //    Assert.AreEqual(1, boxer.Statistics.Knockouts);
    //    Assert.AreEqual(1, boxer.Statistics.WinStreak);
    //    Assert.AreEqual(1, boxer.Statistics.BestWinStreak);
    //    Assert.IsNotNull(boxer.Statistics.LastFightDate);
    //    Assert.AreEqual(ExpectedRating, boxer.Ranking.EloRating);
    //    Assert.AreEqual(100, boxer.Statistics.WinRate);
    //    Assert.AreEqual(100, boxer.Statistics.KnockoutRate);
    //}

    //[TestMethod]
    //public void ApplyFightResult_Loss_ShouldUpdateStatistics()
    //{
    //    // Arrange
    //    var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
    //    const int ExpectedRating = 1468;

    //    // Act
    //    boxer.ApplyFightResult(FightResult.Loss, FightEndType.Decision, ExpectedRating);

    //    // Assert
    //    Assert.AreEqual(0, boxer.Statistics.Wins);
    //    Assert.AreEqual(1, boxer.Statistics.Losses);
    //    Assert.AreEqual(0, boxer.Statistics.Draws);
    //    Assert.AreEqual(1, boxer.Statistics.TotalFights);
    //    Assert.AreEqual(0, boxer.Statistics.WinStreak);
    //    Assert.AreEqual(ExpectedRating, boxer.Ranking.EloRating);
    //    Assert.AreEqual(0, boxer.Statistics.WinRate);
    //}

    //[TestMethod]
    //public void ApplyFightResult_Draw_ShouldUpdateStatistics()
    //{
    //    // Arrange
    //    var boxer = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
    //    const int ExpectedRating = 1500;

    //    // Act
    //    boxer.ApplyFightResult(FightResult.Draw, FightEndType.Draw, ExpectedRating);

    //    // Assert
    //    Assert.AreEqual(0, boxer.Statistics.Wins);
    //    Assert.AreEqual(0, boxer.Statistics.Losses);
    //    Assert.AreEqual(1, boxer.Statistics.Draws);
    //    Assert.AreEqual(1, boxer.Statistics.TotalFights);
    //    Assert.AreEqual(0, boxer.Statistics.WinStreak);
    //    Assert.AreEqual(ExpectedRating, boxer.Ranking.EloRating);
    //    Assert.AreEqual(0, boxer.Statistics.WinRate);
    //}

    //[TestMethod]
    //public void ApplyFightResult_WithKnockoutLoss_ShouldTrackKnockoutLosses()
    //{
    //    // Arrange
    //    var boxer1 = new Boxer("Mike", "Doe", DateTime.UtcNow.AddYears(-30), 99);
    //    var boxer2 = new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-25), 100);
    //    var fight = new Fight(boxer1.Id, boxer2.Id, DateTime.UtcNow);
    //    fight.Start();
    //    fight.StartRound();
    //    fight.RegisterEvent(new RoundEvent(RoundEventType.RoundEnd, boxer1.Id));
    //    fight.EndCurrentRound(new RoundScore(1,0), );
    //    fight.Complete();

    //    // Act
    //    boxer2.ApplyFightResult(FightResult.Loss, FightEndType.Knockout, 1468);

    //    // Assert
    //    Assert.AreEqual(0, boxer.Statistics.Knockouts);
    //    Assert.AreEqual(1, boxer.Statistics.KnockoutLosses);
    //    Assert.AreEqual(0, boxer.Statistics.WinRate);
    //}
}

