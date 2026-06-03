using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Validators;

namespace PassportOffice.ValueObjects;

public sealed class DepartmentCode : ValueObject<string>
{
    private static readonly DepartmentCodeValidator Validator = new();

    private DepartmentCode()
    {
    }

    public DepartmentCode(string value) : base(Validator, value?.Trim() ?? string.Empty)
    {
    }
}

