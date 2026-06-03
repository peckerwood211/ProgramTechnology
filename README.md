# Паспортный стол

Тема работы: информационная система предметной области «Паспортный стол».

Проект реализован на .NET 8 в многослойной структуре: `Domain`, `Application`, `Infrastructure`, `Presentation`, `Tests`.

Структура:

- `Domain` - сущности, value objects, исключения, абстракции репозиториев, DomainApp и диаграммы.
- `Application` - DTO-контракты и application-сервисы.
- `Infrastructure` - EF Core InMemory, конфигурации сущностей, репозитории, seed-данные.
- `Presentation` - Web API с Minimal API и Swagger.
- `Tests` - xUnit-тесты домена и application-сервисов.

Диаграммы: `Domain/ERD.png`, `Domain/EntitiesClassDiagram.png`, `Domain/Use-caseDiagram.png`;

Запуск:

```bash
dotnet test PassportOffice.sln
dotnet run --project Presentation/PassportOffice.WebHost/PassportOffice.WebHost.csproj
```

