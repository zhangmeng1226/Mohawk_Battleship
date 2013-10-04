namespace MBC.Core.Events
{
    public interface IEventGenerator
    {
        event MBCEventHandler EventGenerated;
    }
}