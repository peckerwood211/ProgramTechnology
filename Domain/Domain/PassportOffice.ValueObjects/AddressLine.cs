using System.Text.RegularExpressions;
using PassportOffice.ValueObjects.Base;
using PassportOffice.ValueObjects.Validators;

namespace PassportOffice.ValueObjects;

public sealed class AddressLine : ValueObject<string>
{
    private static readonly AddressLineValidator Validator = new();

    private AddressLine()
    {
    }

    public AddressLine(string value) : base(Validator, Normalize(value))
    {
    }

    private static string Normalize(string value)
        => Regex.Replace(value?.Trim() ?? string.Empty, @"\s+", " ");
}

