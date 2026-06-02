namespace Domain.ValueObjects;

public sealed class Placa : IEquatable<Placa>
{
    public string Value { get; }

    private Placa(string value) => Value = value;

    public static Placa Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("La placa no puede estar vacía.", nameof(value));

        var cleaned = value.Trim().ToUpperInvariant();
        if (cleaned.Length < 5 || cleaned.Length > 10)
            throw new ArgumentException("La placa debe tener entre 5 y 10 caracteres.", nameof(value));

        return new Placa(cleaned);
    }

    public bool Equals(Placa? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as Placa);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static bool operator ==(Placa? left, Placa? right) => Equals(left, right);
    public static bool operator !=(Placa? left, Placa? right) => !Equals(left, right);
}
