using System.Text.RegularExpressions;
using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Validators;

namespace PassportOffice.ValueObjects;

public sealed class FullName : ValueObject<string>
{
    private static readonly FullNameValidator Validator = new();

    private FullName()
    {
    }

    public FullName(string value) : base(Validator, Normalize(value))
    {
    }

    private static string Normalize(string value)
        => Regex.Replace(value?.Trim() ?? string.Empty, @"\s+", " ");
}

