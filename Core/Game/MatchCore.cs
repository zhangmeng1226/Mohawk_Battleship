using MBC.Core.Controllers;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Entities;
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
    [Configuration("mbc_player_timeout", 500)]
    /// <summary>
    /// This is the new framework part of a match.
    /// </summary>
    public class MatchCore : Match
    {
        /// <summary>
        /// Creates a match with parameters loaded from a configuration.
        /// </summary>
        /// <param name="config"></param>
        public MatchCore(Configuration config)
            : base((Entity)null)
        {
            Events = new List<Event>();
            Players = new HashSet<Player>();
            Teams = new HashSet<Team>();
            Random = new Random();
            OnEvent += ApplyEvent;

            ApplyParameters(config);
        }

        public MatchCore()
            : this(Configuration.Global)
        {
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

        /// <summary>
        /// Moves the match progress forward at a standard pace.
        /// </summary>
        public virtual void Play()
        {
            GameTimer.Start();
            try
            {
                if (!IsRunning)
                {
                    IsRunning = true;
                    if (CurrentRound == -1)
                    {
                        Begin();
                        RoundBegin();
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
                            RoundBegin();
                        }
                    }
                }
            }
            finally
            {
                GameTimer.Stop();
            }
        }

        /// <summary>
        /// Adds a Player from an IController to the match.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public void PlayerCreate(ControllerSkeleton skeleton)
        {
            ControlledPlayer newPlayer = new ControlledPlayer(this, skeleton.GetAttribute<NameAttribute>().Name, skeleton.CreateInstance());
            newPlayer.OnEvent += ApplyEvent;
            PlayerAdd(newPlayer);
            foreach (Ship ship in newPlayer.Ships)
            {
                ship.OnEvent += ApplyEvent;
            }
            newPlayer.Controller.Player = newPlayer;
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
        /// Moves the current event index to the last event, and applies the given event.
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        protected void ApplyEvent(Event ev)
        {
            //PlayToLastEvent();
            Events.Add(ev);
        }

        /// <summary>
        /// Must be overriden by the sub class that provides round functionality.
        /// </summary>
        /// <returns></returns>
        protected virtual bool PlayLogic()
        {
            return false;
        }

        /// Applies a configuration to the match.
        /// </summary>
        /// <param name="conf"></param>
        private void ApplyParameters(Configuration conf)
        {
            FieldSize = new Coordinates(conf.GetValue<int>("mbc_field_width"), conf.GetValue<int>("mbc_field_height"));
            NumberOfRounds = conf.GetValue<int>("mbc_match_rounds");

            StartingShips = new HashSet<Ship>();
            foreach (int length in conf.GetList<int>("mbc_ship_sizes"))
            {
                StartingShips.Add(new Ship(this, length));
            }
            TimeLimit = conf.GetValue<int>("mbc_player_timeout");
        }
    }
}