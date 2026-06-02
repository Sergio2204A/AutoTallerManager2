namespace Domain.ValueObjects;

public sealed class Cedula : IEquatable<Cedula>
{
    public string Value { get; }

    private Cedula(string value) => Value = value;

    public static Cedula Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("La cédula no puede estar vacía.", nameof(value));

        var cleaned = value.Trim().Replace(".", "").Replace("-", "");
        if (cleaned.Length < 5 || cleaned.Length > 15)
            throw new ArgumentException("La cédula debe tener entre 5 y 15 dígitos.", nameof(value));

        return new Cedula(cleaned);
    }

    public bool Equals(Cedula? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as Cedula);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static bool operator ==(Cedula? left, Cedula? right) => Equals(left, right);
    public static bool operator !=(Cedula? left, Cedula? right) => !Equals(left, right);
}
