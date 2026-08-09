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
    public void ThrowExceptionWhenCreatingBoxerWithInvalidBirthDate()
    {
        Assert.ThrowsExactly<DomainException>(() => new Boxer("John", "Doe", DateTime.UtcNow.AddYears(5), 100));
        Assert.ThrowsExactly<DomainException>(() => new Boxer("John", "Doe", DateTime.UtcNow.AddYears(-1), 100));
    }

    //[TestMethod]
    //public void RegisterWinWithValidParameters()
    //{

    //}
}

