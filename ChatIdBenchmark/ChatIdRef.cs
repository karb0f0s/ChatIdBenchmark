#nullable disable
public class ChatIdRef
{
    public readonly long Identifier;
    public readonly string Username;

    public ChatIdRef(long identifier)
    {
        Identifier = identifier;
    }

    public ChatIdRef(string username)
    {
        if (username.Length > 1 && username[0] == '@')
        {
            Username = username;
        }
        else
        {
            long.TryParse(username, out Identifier);
        }
    }

    public override bool Equals(object obj) => ((string)this).Equals(obj);
    public override int GetHashCode() => ((string)this).GetHashCode();

    public static implicit operator ChatIdRef(long identifier) => new(identifier);
    public static implicit operator ChatIdRef(string username) => new(username);

    public override string ToString() => this;
    public static implicit operator string(ChatIdRef chatid) => chatid.Username ?? chatid.Identifier.ToString();
    //public static implicit operator ChatIdRef(Chat chat) =>
    //    chat.Id != default ? chat.Id : (ChatIdRef)("@" + chat.Username);
}
#nullable enable
