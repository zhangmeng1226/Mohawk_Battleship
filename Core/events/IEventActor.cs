namespace MBC.Core.Events
{
    public interface IEventActor
    {
        void ReflectEvent(Event ev);
    }
}