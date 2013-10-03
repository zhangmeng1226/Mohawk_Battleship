using MBC.Core.Events;
using MBC.Shared;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBC.Core.Rounds
{

    public abstract class Round : ISerializable 
    {
        private List<Event> events;
        private int currentEventIdx;

        protected Dictionary<IDNumber, Team> roundTeams;

        protected Dictionary<IDNumber, FieldInfo> playerField;
        protected Dictionary<IDNumber, Register> registers;

        public bool IsRunning
        {
            get;
            private set;
        }

        public event MBCEventHandler EventCreated;

        public Round()
        {
            events = new List<Event>();
            playerField = new Dictionary<IDNumber, FieldInfo>();
            registers = new Dictionary<IDNumber, Register>();
            roundTeams = new Dictionary<IDNumber, Team>();
        }

        public Round(MatchConfig config)
        {

        }

        public Round(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
        }

        protected virtual void GenerateEvent(Event ev)
        {
            events.Add(ev);
            if (EventCreated != null)
            {
                EventCreated(ev);
            }
        }

        public abstract void Play();

        public abstract void Stop();

        protected virtual void ReflectEvent(Event re)
        {
            switch (re.EventType)
            {
                case Event.Type.MatchAddPlayer:
                    var addPlayerEvent = (MatchAddPlayerEvent)re;
                    playerField[addPlayerEvent.PlayerID] = new FieldInfo();
                    registers[addPlayerEvent.PlayerID] = new Register(addPlayerEvent.PlayerID, addPlayerEvent.PlayerName);
                    break;
                case Event.Type.MatchRemovePlayer:
                    var removePlayerEvent = (MatchRemovePlayerEvent)re;
                    playerField.Remove(removePlayerEvent.PlayerID);
                    registers.Remove(removePlayerEvent.PlayerID);
                    foreach (var team in roundTeams)
                    {
                        team.Value.Members.Add(removePlayerEvent.PlayerID);
                    }
                    break;
                case Event.Type.MatchTeamCreate:
                    var teamCreateEvent = (MatchTeamCreateEvent)re;
                    roundTeams.Add(new Team(teamCreateEvent.TeamID, teamCreateEvent.TeamName));
                    break;
            }
        }
    }
}