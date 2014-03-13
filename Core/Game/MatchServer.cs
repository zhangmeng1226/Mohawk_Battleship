using MBC.Core.Controllers;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MBC.Core.Game
{
    [Configuration("mbc_field_width", 10)]
    [Configuration("mbc_field_height", 10)]
    [Configuration("mbc_ship_sizes", 2, 3, 3, 4, 5)]
    [Configuration("mbc_game_mode", GameMode.Classic, null)]
    [Configuration("mbc_match_playeradd_init_only", true)]
    [Configuration("mbc_match_rounds_mode", RoundMode.AllRounds,
        Description = "Determines the ending behaviour of a match based on a given number of rounds.",
        DisplayName = "Match Rounds Mode")]
    [Configuration("mbc_match_rounds", 100)]
    [Configuration("mbc_match_listen_port", 49628)]
    /// <summary>
    /// This is the new framework part of a match.
    /// </summary>
    public class MatchServer : Match
    {
        public static Coordinates COORDS_ZERO = new Coordinates(0, 0);

        private Socket serverSocket;

        /// <summary>
        /// Creates a match with parameters loaded from a configuration.
        /// </summary>
        /// <param name="config"></param>
        public MatchServer(Configuration config)
        {
            Events = new List<Event>();
            Players = new HashSet<Player>();
            Teams = new HashSet<Team>();
            Random = new Random();
            GameTimer = new Stopwatch();
            CurrentRound = -1;
            OnMatchBegin += Event_OnMatchBegin;
            OnMatchEnd += Event_OnMatchEnd;
            OnPlayerAdd += Event_OnPlayerAdd;
            OnPlayerDisqualified += Event_OnPlayerDisqualified;
            OnPlayerLose += Event_OnPlayerLose;
            OnPlayerMessage += Event_OnPlayerMessage;
            OnPlayerRemove += Event_OnPlayerRemove;
            OnPlayerShipDestruction += Event_OnPlayerShipDestruction;
            OnPlayerShipsPlaced += Event_OnPlayerShipsPlaced;
            OnPlayerShot += Event_OnPlayerShot;
            OnPlayerTeamAssign += Event_OnPlayerTeamAssign;
            OnPlayerTimeout += Event_OnPlayerTimeout;
            OnPlayerTurnSwitch += Event_OnPlayerTurnSwitch;
            OnPlayerWin += Event_OnPlayerWin;
            OnRoundBegin += Event_OnRoundBegin;
            OnRoundEnd += Event_OnRoundEnd;
            OnTeamAdd += Event_OnTeamAdd;
            OnTeamRemove += Event_OnTeamRemove;

            ApplyParameters(config);

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public MatchServer()
            : this(Configuration.Global)
        {
        }

        public List<NetController> clients
        {
            get;
            private set;
        }

        public int CurrentEvent
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the events that have been generated within the match.
        /// </summary>
        public List<Event> Events
        {
            get;
            protected set;
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        protected Stopwatch GameTimer
        {
            get;
            private set;
        }

        /// <summary>
        /// Determines whether or not a shot made by a player is valid.
        /// </summary>
        /// <param name="shooter"></param>
        /// <param name="shot"></param>
        /// <returns></returns>
        public bool IsShotValid(Player shooter, Shot shot)
        {
            return ShotList.IsShotMade(shooter.ShotsMade, shot) &&
                shot.Coordinates > COORDS_ZERO &&
                shot.Coordinates < FieldSize;
        }

        /// <summary>
        /// Moves the match progress forward at a standard pace.
        /// </summary>
        public virtual void Play()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                if (CurrentRound == -1)
                {
                    if (!ApplyEvent(RoundBegin(CurrentRound + 1)))
                    {
                        IsRunning = false;
                    }
                }
                while (IsRunning && !AtEnd)
                {
                    var isRoundPlaying = PlayLogic();
                    if ((!isRoundPlaying) && (NumberOfRounds - CurrentRound == 0))
                    {
                        End();
                        break;
                    }
                    else if (!isRoundPlaying)
                    {
                        RoundEnd(CurrentRound);
                        RoundBegin(++CurrentRound);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the match events either forward or backward to a specific
        /// event index.
        /// </summary>
        /// <param name="eventIdx"></param>
        /// <returns></returns>
        public bool PlayToEvent(int eventIdx)
        {
            if (eventIdx < 0 || eventIdx >= Events.Count)
            {
                return false;
            }
            if (eventIdx < CurrentEvent)
            {
                /*
                for (; CurrentEvent >= eventIdx; CurrentEvent--)
                {
                    if (!Events[CurrentEvent].ApplyBackward())
                    {
                        return false;
                    }
                    NotifyEvent(Events[CurrentEvent]);
                    if (CurrentEvent == eventIdx)
                    {
                        break;
                    }
                }*/
                throw new NotImplementedException("No backward-playing functionality.");
            }
            else if (eventIdx > CurrentEvent)
            {
                /*
                for (; CurrentEvent <= eventIdx; CurrentEvent++)
                {
                    if (!Events[CurrentEvent].ApplyForward())
                    {
                        return false;
                    }
                    NotifyEvent(Events[CurrentEvent]);
                    if (CurrentEvent == eventIdx)
                    {
                        break;
                    }
                }*/
                throw new NotImplementedException("No replaying functionality.");
            }
            return true;
        }

        /// <summary>
        /// Plays the match through to the last event that was generated.
        /// </summary>
        /// <returns></returns>
        public bool PlayToLastEvent()
        {
            return PlayToEvent(Events.Count - 1);
        }

        /// <summary>
        /// Stops progression of the match.
        /// </summary>
        public virtual void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Finds the first IDNumber that is unoccupied in the players list.
        /// </summary>
        /// <returns></returns>
        internal int FindFirstEmptyPlayerID()
        {
            var numberSet = new HashSet<int>();
            foreach (var plr in Players)
            {
                numberSet.Add(plr.ID);
            }
            for (int i = 0; i < numberSet.Count; i++)
            {
                if (numberSet.Contains(i))
                {
                    return i;
                }
            }
            return numberSet.Count;
        }

        /// <summary>
        /// Moves the current event index to the last event, and applies the given event.
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        protected bool ApplyEvent(Event ev)
        {
            PlayToLastEvent();
            Events.Add(ev);
            ev.Millis = (int)(GameTimer.ElapsedMilliseconds);
            return true;
        }

        /// <summary>
        /// Must be overriden by the sub class that provides round functionality.
        /// </summary>
        /// <returns></returns>
        protected virtual bool PlayLogic()
        {
            return false;
        }

        /// <summary>
        /// Applies a configuration to the match.
        /// </summary>
        /// <param name="conf"></param>
        private void ApplyParameters(Configuration conf)
        {
            FieldSize = new Coordinates(conf.GetValue<int>("mbc_field_width"), conf.GetValue<int>("mbc_field_height"));
            NumberOfRounds = conf.GetValue<int>("mbc_match_rounds");

            StartingShips = ShipList.ShipsFromLengths(conf.GetList<int>("mbc_ship_sizes"));
            TimeLimit = conf.GetValue<int>("mbc_player_timeout");
        }

        private void Event_OnMatchBegin(object sender, MatchBeginEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnMatchEnd(object sender, MatchEndEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerAdd(object sender, MatchAddPlayerEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerDisqualified(object sender, PlayerDisqualifiedEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerLose(object sender, PlayerLostEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerMessage(object sender, PlayerMessageEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerRemove(object sender, MatchRemovePlayerEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerShipDestruction(object sender, PlayerShipDestroyedEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerShipsPlaced(object sender, PlayerShipsPlacedEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerShot(object sender, PlayerShotEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerTeamAssign(object sender, PlayerTeamAssignEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerTimeout(object sender, PlayerTimeoutEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerTurnSwitch(object sender, PlayerTurnSwitchEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnPlayerWin(object sender, PlayerWonEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnRoundBegin(object sender, RoundBeginEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnRoundEnd(object sender, RoundEndEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnTeamAdd(object sender, MatchTeamAddEvent e)
        {
            ApplyEvent(e);
        }

        private void Event_OnTeamRemove(object sender, MatchTeamRemoveEvent e)
        {
            ApplyEvent(e);
        }
    }
}