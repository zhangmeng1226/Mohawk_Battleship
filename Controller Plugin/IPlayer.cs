namespace MBC.Shared
{
    public interface IPlayer
    {
        Register Register { get; set; }

        FieldInfo Field { get; set; }

        RegisterInfo Stats { get; set; }

        MatchInfo Match { get; set; }

        Team Team { get; set; }
    }
}