using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Validators;

namespace PassportOffice.ValueObjects;

public sealed class PassportSeries : ValueObject<string>
{
    private static readonly PassportSeriesValidator Validator = new();

    private PassportSeries()
    {
    }

    public PassportSeries(string value) : base(Validator, value?.Trim() ?? string.Empty)
    {
    }
}

