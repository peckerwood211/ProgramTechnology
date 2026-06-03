using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Enums;
using PassportOffice.Domain.Exceptions;
using PassportOffice.ValueObjects;
using PassportOffice.ValueObjects.Exceptions;

namespace PassportOffice.Tests;

public class DomainWorkflowTests
{
    [Fact]
    public void IssuePassport_ReplacesPreviousActivePassport()
    {
        var citizen = CreateCitizen();

        var firstPassport = citizen.IssuePassport(
            new PassportSeries("2519"),
            new PassportNumber("111111"),
            new DepartmentCode("380-001"),
            "Central passport office",
            new DateOnly(2019, 1, 1));

        var secondPassport = citizen.IssuePassport(
            new PassportSeries("2524"),
            new PassportNumber("222222"),
            new DepartmentCode("380-001"),
            "Central passport office",
            new DateOnly(2024, 1, 1));

        Assert.Equal(PassportStatus.Replaced, firstPassport.Status);
        Assert.Equal(PassportStatus.Active, secondPassport.Status);
        Assert.Equal(secondPassport, citizen.CurrentPassport);
    }

    [Fact]
    public void TemporaryRegistration_RequiresEndDate()
    {
        var citizen = CreateCitizen();

        Assert.Throws<RegistrationPeriodException>(() =>
            citizen.RegisterAddress(
                new AddressLine("Irkutsk, Sovetskaya street, building 2"),
                RegistrationType.Temporary,
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow)));
    }

    [Theory]
    [InlineData("25")]
    [InlineData("250")]
    [InlineData("ABCD")]
    public void PassportSeries_ValidatesFormat(string series)
        => Assert.Throws<ValueObjectValidationException>(() => new PassportSeries(series));

    private static Citizen CreateCitizen()
        => new(
            Guid.NewGuid(),
            new FullName("Sidorov Ivan Petrovich"),
            new DateOnly(1990, 1, 1),
            "Irkutsk",
            Gender.Male,
            "123-456-789 00");
}
