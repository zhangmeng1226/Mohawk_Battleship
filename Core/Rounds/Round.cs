﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;
using MBC.Core.Accolades;
using MBC.Core.Events;
using MBC.Shared;

namespace MBC.Core
{

    /// <summary>
    /// A Round class is the base class for a battleship round that processes game logic based on
    /// decisions made by controllers. A Round can support any number of Controller objects.<br /><br />
    /// In each Round, the following rules apply and are enforced, not overridable by any derived Round class:
    /// <list type="bullet">
    /// <item>Controllers must not overlap their ships.</item>
    /// <item>Controllers must return from any of their called methods within the game time limit.</item>
    /// </list>
    /// If any of the above rules are broken, the controller loses the Round.
    /// </summary>
    public abstract class Round
    {

        protected ControllerRegister currentTurn;

        protected List<ControllerRegister> registers;

        protected List<ControllerRegister> remainingRegisters;

        /// <summary>
        /// The current game state of this Round.
        /// </summary>
        protected State currentState;

        /// <summary>
        /// The Events generated and accumulated by this Round.
        /// </summary>
        private List<Event> generatedEvents;

        /// <summary>
        /// The MatchInfo object that contains properties that dictate how this Round behaves.
        /// </summary>
        private MatchInfo matchInfo;

        /// <summary>
        /// The AccoladeGenerator that generates and adds Accolade objects to this Round.
        /// </summary>
        private AccoladeGenerator accoladeGenerator;

        /// <summary>
        /// Constructs a Round with the given Player objects.
        /// </summary>
        /// <param name="inputRegisters">A variable number of controllers that are involved in this Round.</param>
        /// <param name="matchInfo">Information about the match that determines Round behaviour.</param>
        public Round(MatchInfo matchInfo, List<ControllerRegister> inputRegisters)
        {
            generatedEvents = new List<Event>();
            registers = new List<ControllerRegister>();
            remainingRegisters = new List<ControllerRegister>();
            accoladeGenerator = new AccoladeGenerator(this);

            Accolades = new List<Accolade>();

            //Set the state to Begin.
            currentState = State.Begin;

            //Monitor events created.
            RoundEvent += (ev) =>
            {
                generatedEvents.Add(ev);
            };
            ControllerEvent += (ev) =>
            {
                generatedEvents.Add(ev);
            };

            this.matchInfo = matchInfo;

            //Populate the Controller and RemainingController lists.
            foreach (var register in inputRegisters)
            {
                registers.Add(register);
                remainingRegisters.Add(register);
            }
        }

        /// <summary>
        /// Invoked when an event has been generated by this Round.
        /// </summary>
        public event MBCRoundEventHandler RoundEvent;

        public event MBCControllerEventHandler ControllerEvent;

        /// <summary>
        /// Identifies the time in the game this Round is in.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// The Round is beginning.
            /// </summary>
            Begin,

            /// <summary>
            /// The Controller objects in the Round are to make their ship placements.
            /// </summary>
            ShipPlacement,

            /// <summary>
            /// The Round is progressing through the main game logic.
            /// </summary>
            Main,

            /// <summary>
            /// The Round has ended.
            /// </summary>
            End
        }

        /// <summary>
        /// Gets the current State of this Round.
        /// </summary>
        public State CurrentState
        {
            get
            {
                return currentState;
            }
        }

        /// <summary>
        /// Gets the MatchInfo associated with this Round.
        /// </summary>
        public MatchInfo MatchInfo
        {
            get
            {
                return matchInfo;
            }
        }

        public int RemainingCount
        {
            get
            {
                return RemainingRegisters.Count;
            }
        }

        public int ControllerCount
        {
            get
            {
                return Registers.Count;
            }
        }

        public List<Event> Events
        {
            get
            {
                return generatedEvents;
            }
        }

        public List<ControllerRegister> Registers
        {
            get
            {
                return registers;
            }
        }

        public List<ControllerRegister> RemainingRegisters
        {
            get
            {
                return remainingRegisters;
            }
        }

        /// <summary>
        /// A List of Accolade objects that have been added.
        /// </summary>
        protected List<Accolade> Accolades { get; set; }

        /// <summary>
        /// Used to progress a Round. Returns a value indicating whether or not this Round can be
        /// progressed any further.
        /// </summary>
        /// <returns>true if the Round is in progress. false otherwise.</returns>
        public bool Progress()
        {
            switch (currentState)
            {
                case State.Begin:
                    Begin();
                    break;
                case State.ShipPlacement:
                    ShipPlacement();
                    break;
                case State.Main:
                    DoMainLogic();
                    break;
            }
            return currentState != State.End;
        }

        public void AddAccolade(Accolade accolade)
        {
            Accolades.Add(accolade);
            MakeEvent(new RoundAccoladeEvent(this, accolade));
        }

        /// <summary>
        /// Ends the Round. Fires the RoundEndEvent. Sets the State of this Round to End. Removes all event
        /// listeners for this Round.
        /// </summary>
        public virtual void End()
        {
            RoundEvent = null;
            ControllerEvent = null;
        }

        /// <summary>
        /// Switches the turn from the current Player to the next Player. The current Player must exist in the
        /// remaining Player list or this method will throw an exception.
        /// </summary>
        /// <exception cref="InvalidOperationException">The current player had been removed the existing player list before switching turns.</exception>
        protected void NextTurn()
        {
            currentTurn = NextRemaining();
        }

        protected ControllerRegister NextRemaining()
        {
            if (currentTurn == null)
            {
                return null;
            }

            var next = (currentTurn.ID + 1) % registers.Count;
            while (next != currentTurn.ID)
            {
                foreach (var register in remainingRegisters)
                {
                    if (register.ID == next)
                    {
                        return register;
                    }
                }
                next = (next + 1) % registers.Count;
            }
            return null;
        }

        /// <summary>
        /// Determines if the ships associated with the given Entity are placed, and do not conflict with each other.
        /// </summary>
        /// <param name="register">The Entity ships to check</param>
        /// <returns>true if the ships related to the given Entity are valid, false otherwise.</returns>
        protected bool ControllerShipsValid(ControllerRegister register)
        {
            return register.Ships != null && register.Ships.ShipsPlaced && register.Ships.GetConflictingShips().Count == 0;
        }

        /// <summary>
        /// Invokes all event listeners on the RoundEvent event in this Round with the given RoundEvent object.
        /// </summary>
        /// <param name="ev">The RoundEvent to pass to the event listeners.</param>
        protected void MakeEvent(RoundEvent ev)
        {
            if (RoundEvent != null)
            {
                RoundEvent(ev);
            }
        }

        protected void MakeEvent(ControllerEvent ev)
        {
            if (RoundEvent != null)
            {
                ControllerEvent(ev);
            }
        }

        /// <summary>
        /// The main game logic method invoked during normal round play.
        /// </summary>
        protected abstract void DoMainLogic();

        /// <summary>
        /// Invokes the controller's NewRound method and constructs new ShotList objects for each Player. Picks
        /// a random Player from the Player List to start the Round off. Sets the State of this Round to
        /// ShipPlacement. Fires the RoundBeginEvent.<br />
        /// The base class's implementation for beginning a round is standard.
        /// </summary>
        protected abstract void Begin();

        /// <summary>
        /// Invokes the Player Controller object's methods for ship placement. Switches the State of this Round to
        /// the Shooting State if all Player Controller objects have made valid ship placements. Removes a Player
        /// from the List of remaining Player objects if their Controller does not make valid Ship placements
        /// or they run out of time.<br/>
        /// The base class's implemented procedure for ship placement is standard.
        /// </summary>
        protected abstract void ShipPlacement();
    }
}
