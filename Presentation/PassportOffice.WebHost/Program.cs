using System.Text.Json.Serialization;
using PassportOffice.Application.Contracts.Applications;
using PassportOffice.Application.Contracts.Appointments;
using PassportOffice.Application.Contracts.Citizens;
using PassportOffice.Application.Contracts.Passports;
using PassportOffice.Application.Contracts.Services;
using PassportOffice.Application.Services;
using PassportOffice.Domain.Enums;
using PassportOffice.Infrastructure.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration.GetValue<string>("DatabaseName") ?? "PassportOfficeDb");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DatabaseSeeder.SeedAsync(db);
}

app.MapGet("/", () => Results.Ok(new
{
    name = "PassportOffice",
    description = "Course project API for a passport office domain.",
    swagger = "/swagger"
}));

var citizens = app.MapGroup("/api/citizens").WithTags("Citizens");

citizens.MapGet("/", async (string? query, ICitizensApplicationService service, CancellationToken cancellationToken) =>
{
    var result = string.IsNullOrWhiteSpace(query)
        ? await service.GetAllAsync(cancellationToken)
        : await service.SearchAsync(query, cancellationToken);

    return Results.Ok(result);
});

citizens.MapGet("/{id:guid}", async (Guid id, ICitizensApplicationService service, CancellationToken cancellationToken) =>
    await service.GetByIdAsync(id, cancellationToken) is { } citizen
        ? Results.Ok(citizen)
        : Results.NotFound());

citizens.MapPost("/", async (CreateCitizenModel model, ICitizensApplicationService service, CancellationToken cancellationToken) =>
{
    var citizen = await service.CreateAsync(model, cancellationToken);
    return Results.Created($"/api/citizens/{citizen!.Id}", citizen);
});

citizens.MapPut("/{id:guid}/contacts", async (Guid id, UpdateCitizenContactsModel model, ICitizensApplicationService service, CancellationToken cancellationToken) =>
    await service.UpdateContactsAsync(id, model, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

citizens.MapPut("/{id:guid}/name", async (Guid id, ChangeCitizenNameModel model, ICitizensApplicationService service, CancellationToken cancellationToken) =>
    await service.ChangeNameAsync(id, model, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

citizens.MapPost("/{id:guid}/passports", async (Guid id, IssuePassportModel model, ICitizensApplicationService service, CancellationToken cancellationToken) =>
    await service.IssuePassportAsync(model with { CitizenId = id }, cancellationToken) is { } passport
        ? Results.Created($"/api/citizens/{id}", passport)
        : Results.Conflict("Citizen not found or passport number already exists."));

citizens.MapPost("/{id:guid}/registrations", async (Guid id, RegisterAddressModel model, ICitizensApplicationService service, CancellationToken cancellationToken) =>
    await service.RegisterAddressAsync(model with { CitizenId = id }, cancellationToken) is { } registration
        ? Results.Created($"/api/citizens/{id}", registration)
        : Results.NotFound());

var applications = app.MapGroup("/api/applications").WithTags("Applications");

applications.MapGet("/", async (ApplicationStatus? status, IPassportApplicationsService service, CancellationToken cancellationToken) =>
{
    var result = status is null
        ? await service.GetAllAsync(cancellationToken)
        : await service.GetByStatusAsync(status.Value, cancellationToken);

    return Results.Ok(result);
});

applications.MapGet("/{id:guid}", async (Guid id, IPassportApplicationsService service, CancellationToken cancellationToken) =>
    await service.GetByIdAsync(id, cancellationToken) is { } application
        ? Results.Ok(application)
        : Results.NotFound());

applications.MapPost("/", async (CreatePassportApplicationModel model, IPassportApplicationsService service, CancellationToken cancellationToken) =>
    await service.CreateAsync(model, cancellationToken) is { } application
        ? Results.Created($"/api/applications/{application.Id}", application)
        : Results.BadRequest("Citizen, department or service was not found."));

applications.MapPost("/{id:guid}/accept", async (Guid id, ApplicationDecisionModel model, IPassportApplicationsService service, CancellationToken cancellationToken) =>
    await service.AcceptAsync(id, model, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

applications.MapPost("/{id:guid}/approve", async (Guid id, ApplicationDecisionModel model, IPassportApplicationsService service, CancellationToken cancellationToken) =>
    await service.ApproveAsync(id, model, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

applications.MapPost("/{id:guid}/reject", async (Guid id, ApplicationDecisionModel model, IPassportApplicationsService service, CancellationToken cancellationToken) =>
    await service.RejectAsync(id, model, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

applications.MapPost("/{id:guid}/cancel", async (Guid id, CancelApplicationModel model, IPassportApplicationsService service, CancellationToken cancellationToken) =>
    await service.CancelAsync(id, model.Reason, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

var appointments = app.MapGroup("/api/appointments").WithTags("Appointments");

appointments.MapGet("/", async (IAppointmentsApplicationService service, CancellationToken cancellationToken) =>
    Results.Ok(await service.GetAllAsync(cancellationToken)));

appointments.MapGet("/{id:guid}", async (Guid id, IAppointmentsApplicationService service, CancellationToken cancellationToken) =>
    await service.GetByIdAsync(id, cancellationToken) is { } appointment
        ? Results.Ok(appointment)
        : Results.NotFound());

appointments.MapGet("/citizen/{citizenId:guid}", async (Guid citizenId, IAppointmentsApplicationService service, CancellationToken cancellationToken) =>
    Results.Ok(await service.GetByCitizenAsync(citizenId, cancellationToken)));

appointments.MapPost("/", async (CreateAppointmentModel model, IAppointmentsApplicationService service, CancellationToken cancellationToken) =>
    await service.CreateAsync(model, cancellationToken) is { } appointment
        ? Results.Created($"/api/appointments/{appointment.Id}", appointment)
        : Results.BadRequest("Citizen, department, employee or application was not found."));

appointments.MapPost("/{id:guid}/confirm", async (Guid id, IAppointmentsApplicationService service, CancellationToken cancellationToken) =>
    await service.ConfirmAsync(id, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

appointments.MapPost("/{id:guid}/complete", async (Guid id, IAppointmentsApplicationService service, CancellationToken cancellationToken) =>
    await service.CompleteAsync(id, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

appointments.MapPost("/{id:guid}/cancel", async (Guid id, IAppointmentsApplicationService service, CancellationToken cancellationToken) =>
    await service.CancelAsync(id, cancellationToken)
        ? Results.NoContent()
        : Results.NotFound());

var departments = app.MapGroup("/api/departments").WithTags("Departments");

departments.MapGet("/", async (IDepartmentsApplicationService service, CancellationToken cancellationToken) =>
    Results.Ok(await service.GetAllAsync(cancellationToken)));

departments.MapGet("/{code}", async (string code, IDepartmentsApplicationService service, CancellationToken cancellationToken) =>
    await service.GetByCodeAsync(code, cancellationToken) is { } department
        ? Results.Ok(department)
        : Results.NotFound());

app.Run();
