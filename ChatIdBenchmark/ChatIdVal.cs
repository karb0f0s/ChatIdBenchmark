using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

//[JsonConverter(typeof(ChatIdConverter))]
public struct ChatIdVal : IEquatable<ChatIdVal>, IEqualityComparer<ChatIdVal>
{
    private static readonly Regex NameValidation = new("^@[a-zA-Z0-9_]{5,}$");

    public readonly string? Username;
    public readonly long? Identifier;

    private ChatIdVal(string? username, long? identifier)
    {
        Username = username;
        Identifier = identifier;
    }

    public ChatIdVal([DisallowNull] string username)
        : this(null, null)
    {
        _ = username ?? throw new ArgumentNullException(nameof(username));

        if (long.TryParse(username, NumberStyles.Integer, null, out long identifier))
        {
            Identifier = identifier;
            return;
        }

        if (NameValidation.IsMatch(username))
        {
            Username = username;
            return;
        }

        throw new ArgumentException(
            $"{nameof(username)} value has to start with '@' symbol and be at least 5 characters long or be a valid long value.",
            nameof(username));
    }

    public ChatIdVal([DisallowNull] long identifier)
        : this(null, identifier)
    { }

    public bool Equals([DisallowNull] ChatIdVal other) =>
        (Username, Identifier) switch
        {
            (not null, null) => other.Username is not null && Username == other.Username,
            (null, not null) => other.Identifier is not null && Identifier == other.Identifier,
            _ => false
        };
    public bool Equals([DisallowNull] ChatIdVal x, ChatIdVal y) => x.Equals(y);
    public override bool Equals(object? obj) => obj is ChatIdVal other && this.Equals(other);
    public static bool operator ==([DisallowNull] ChatIdVal left, ChatIdVal right) => left.Equals(right);
    public static bool operator !=([DisallowNull] ChatIdVal left, ChatIdVal right) => !(left == right);

    public override int GetHashCode() => (Identifier, Username).GetHashCode();
    public int GetHashCode([DisallowNull] ChatIdVal chatId) => chatId.GetHashCode();

    public static implicit operator ChatIdVal([DisallowNull] long identifier) => new(identifier);
    public static implicit operator ChatIdVal([DisallowNull] string username) => new(username);

    public override string ToString() => this;
    public static implicit operator string([DisallowNull] ChatIdVal chatId) => chatId.Username ?? chatId.Identifier.ToString() ?? string.Empty;

    //public static implicit operator ChatIdVal(Chat chat)
    //{
    //    _ = chat ?? throw new ArgumentNullException(nameof(chat));

    //    return chat.Id != default
    //        ? chat.Id
    //        : new ChatIdVal($"@{chat.Username}");
    //}
}
