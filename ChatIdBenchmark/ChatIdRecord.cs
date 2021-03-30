using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

public abstract record ChatIdRecord : IEquatable<ChatIdRecord>
{
    private static readonly Regex NameValidation = new("^@[a-zA-Z0-9_]{5,}$");

    public record Username(string Value) : ChatIdRecord;
    public record Identifier(long Value) : ChatIdRecord;

    public static ChatIdRecord New(long value) => new Identifier(value);
    public static ChatIdRecord New(string username)
    {
        _ = username ?? throw new ArgumentNullException(nameof(username));

        //if (long.TryParse(username, NumberStyles.Integer, null, out long identifier))
        //{
        //    return new Identifier(identifier);
        //}

        if (!NameValidation.IsMatch(username))
        {
            throw new ArgumentException(
                $"{nameof(username)} value has to start with '@' symbol and be at least 5 characters long or be a valid long value.",
                nameof(username));
        }
        return new Username(username);
    }

    public static implicit operator string([DisallowNull] ChatIdRecord chatId) => chatId switch
    {
        Username username => username.Value,
        Identifier identifier => identifier.Value.ToString(),
        _ => string.Empty,
    };

    public static implicit operator long([DisallowNull] ChatIdRecord chatId) => chatId switch
    {
        Identifier identifier => identifier.Value,
        _ => throw new InvalidCastException($"Unable to convert {nameof(Username)} to {nameof(Identifier)}"),
    };
}
