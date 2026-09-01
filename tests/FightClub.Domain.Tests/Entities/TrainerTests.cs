using FightClub.Domain.Entities;
using FightClub.Domain.Exceptions;

namespace FightClub.Domain.Tests.Entities;

[TestClass]
public sealed class TrainerTests
{
    [TestMethod]
    public void CreateTrainerWithValidParametersShouldSucceed()
    {
        // Arrange
        var firstName = "Mike";
        var lastName = "Tyson";
        DateTime birthDate = DateTime.UtcNow.AddYears(-50);

        // Act
        var trainer = new Trainer(firstName, lastName, birthDate);

        // Assert
        Assert.AreEqual(firstName, trainer.FirstName);
        Assert.AreEqual(lastName, trainer.LastName);
        Assert.AreEqual($"{firstName} {lastName}", trainer.FullName);
        Assert.AreEqual(birthDate.Date, trainer.DateOfBirth.Date);
        Assert.AreEqual(50, trainer.Age);
        Assert.AreNotEqual(Guid.Empty, trainer.Id);
    }

    [TestMethod]
    [DataRow(17)] // Слишком молодой
    [DataRow(101)] // Слишком старый
    public void CreateTrainerWithInvalidAgeShouldThrow(int age)
    {
        // Arrange
        DateTime birthDate = DateTime.UtcNow.AddYears(-age);

        // Act & Assert
        Assert.ThrowsExactly<DomainException>(() =>
            new Trainer("Mike", "Tyson", birthDate));
    }

    [TestMethod]
    public void RenameTrainerWithValidNamesShouldUpdate()
    {
        // Arrange
        var trainer = new Trainer("Mike", "Tyson", DateTime.UtcNow.AddYears(-50));

        // Act
        trainer.Rename("Evander", "Holyfield");

        // Assert
        Assert.AreEqual("Evander", trainer.FirstName);
        Assert.AreEqual("Holyfield", trainer.LastName);
    }

    [TestMethod]
    public void RenameTrainerWithOnlyFirstNameShouldNotChangeLastName()
    {
        // Arrange
        var trainer = new Trainer("Mike", "Tyson", DateTime.UtcNow.AddYears(-50));
        var originalLastName = trainer.LastName;

        // Act
        trainer.Rename("Evander", null);

        // Assert
        Assert.AreEqual("Mike", trainer.FirstName); // Не должно измениться
        Assert.AreEqual(originalLastName, trainer.LastName);
    }

    [TestMethod]
    public void ChangeBirthDateWithValidDateShouldUpdateAge()
    {
        // Arrange
        var trainer = new Trainer("Mike", "Tyson", DateTime.UtcNow.AddYears(-50));
        DateTime newBirthDate = DateTime.UtcNow.AddYears(-45);

        // Act
        trainer.ChangeBirthDate(newBirthDate);

        // Assert
        Assert.AreEqual(newBirthDate.Date, trainer.DateOfBirth.Date);
        Assert.AreEqual(45, trainer.Age);
    }

    [TestMethod]
    public void ChangeBirthDateWithNullShouldNotChange()
    {
        // Arrange
        var trainer = new Trainer("Mike", "Tyson", DateTime.UtcNow.AddYears(-50));
        DateTime originalBirthDate = trainer.DateOfBirth;

        // Act
        trainer.ChangeBirthDate(null);

        // Assert
        Assert.AreEqual(originalBirthDate, trainer.DateOfBirth);
    }
}
