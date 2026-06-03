using PassportOffice.Domain.Entities;
using PassportOffice.Domain.Enums;
using PassportOffice.ValueObjects;

var department = new OfficeDepartment(
    Guid.NewGuid(),
    new DepartmentCode("380-010"),
    "Demo passport office",
    new AddressLine("Irkutsk, Baikalskaya street, building 20"),
    "Mon-Fri 09:00-18:00");

var employee = department.AddEmployee(
    new FullName("Smirnova Elena Ivanovna"),
    EmployeePosition.SeniorInspector,
    "DEMO-001");

var service = department.AddService(
    new ServiceCode("REPLACE"),
    "Passport replacement",
    ApplicationType.PassportReplacement,
    300m,
    10);

var citizen = new Citizen(
    Guid.NewGuid(),
    new FullName("Kuznetsov Artem Pavlovich"),
    new DateOnly(1996, 3, 9),
    "Irkutsk",
    Gender.Male,
    "111-222-333 44",
    new PhoneNumber("+7 950 123-45-67"),
    new EmailAddress("artem.kuznetsov@example.com"));

var passport = citizen.IssuePassport(
    new PassportSeries("2520"),
    new PassportNumber("654321"),
    new DepartmentCode("380-010"),
    department.Name,
    new DateOnly(2020, 4, 1));

var registration = citizen.RegisterAddress(
    new AddressLine("Irkutsk, Lermontova street, building 12, apartment 7"),
    RegistrationType.Permanent,
    DateOnly.FromDateTime(DateTime.UtcNow),
    DateOnly.FromDateTime(DateTime.UtcNow));

var application = citizen.SubmitApplication(
    service,
    department,
    ApplicationType.PassportReplacement,
    DateTime.UtcNow,
    "Replacement by age.");


application.AttachDocument(
    AttachedDocumentType.CurrentPassport,
    "Current passport copy",
    passport.FullNumber,
    DateOnly.FromDateTime(DateTime.UtcNow));

application.Accept(employee, DateTime.UtcNow);
application.Approve(employee, "Documents verified", DateTime.UtcNow);

var appointment = citizen.BookAppointment(
    department,
    DateTime.UtcNow.AddDays(3),
    "Passport delivery",
    employee,
    application);
appointment.Confirm();

Console.WriteLine("Passport office domain demo");
Console.WriteLine($"Citizen: {citizen.FullName.Value}");
Console.WriteLine($"Passport: {passport.FullNumber}, status: {passport.Status}");
Console.WriteLine($"Registration: {registration.Address.Value}");
Console.WriteLine($"Application: {application.Number}, status: {application.Status}");
Console.WriteLine($"Appointment: {appointment.ScheduledAt:u}, status: {appointment.Status}");
