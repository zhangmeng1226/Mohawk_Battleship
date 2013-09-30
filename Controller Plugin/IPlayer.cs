namespace MBC.Shared
{
    public interface IPlayer
    {
        Register Register { get; set; }

        FieldInfo Field { get; set; }

        MatchInfo Match { get; set; }

        Team Team { get; set; }
    }
}