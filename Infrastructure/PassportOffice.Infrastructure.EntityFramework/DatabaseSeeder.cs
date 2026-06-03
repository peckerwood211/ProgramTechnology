using Microsoft.EntityFrameworkCore;
using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Enums;
using PassportOffice.ValueObjects;

namespace PassportOffice.Infrastructure.EntityFramework;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Departments.AnyAsync(cancellationToken))
            return;

        var department = new OfficeDepartment(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            new DepartmentCode("380-001"),
            "Central passport office",
            new AddressLine("Irkutsk, Lenina street, building 1"),
            "Mon-Fri 09:00-18:00");

        var inspector = department.AddEmployee(
            new FullName("Ivanova Anna Sergeevna"),
            EmployeePosition.SeniorInspector,
            "EMP-001");

        department.AddEmployee(
            new FullName("Petrov Oleg Viktorovich"),
            EmployeePosition.Registrar,
            "EMP-002");

        var firstPassport = department.AddService(
            new ServiceCode("FIRST-PASSPORT"),
            "First internal passport issuing",
            ApplicationType.FirstPassport,
            300m,
            10);

        department.AddService(
            new ServiceCode("REG-PERM"),
            "Permanent residential registration",
            ApplicationType.PermanentRegistration,
            0m,
            3);

        department.AddService(
            new ServiceCode("REPLACE"),
            "Passport replacement",
            ApplicationType.PassportReplacement,
            300m,
            10);

        var citizen = new Citizen(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            new FullName("Sidorov Pavel Andreevich"),
            new DateOnly(1999, 5, 15),
            "Irkutsk",
            Gender.Male,
            "123-456-789 00",
            new PhoneNumber("+7 999 111-22-33"),
            new EmailAddress("pavel.sidorov@example.com"));

        citizen.IssuePassport(
            new PassportSeries("2519"),
            new PassportNumber("123456"),
            new DepartmentCode("380-001"),
            "Central passport office",
            new DateOnly(2019, 6, 1));

        citizen.RegisterAddress(
            new AddressLine("Irkutsk, Karl Marx street, building 10, apartment 25"),
            RegistrationType.Permanent,
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)),
            DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)));

        var application = citizen.SubmitApplication(
            firstPassport,
            department,
            ApplicationType.FirstPassport,
            DateTime.UtcNow.AddDays(-1),
            "Demo application for course project");

        application.AttachDocument(
            AttachedDocumentType.Photo,
            "Photo 35x45",
            null,
            DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)));

        var appointment = citizen.BookAppointment(
            department,
            DateTime.UtcNow.AddDays(2),
            "Document verification",
            inspector,
            application);
        appointment.Confirm();

        await context.Departments.AddAsync(department, cancellationToken);
        await context.Citizens.AddAsync(citizen, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}

