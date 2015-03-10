using MBC.Core.Controllers;
using MBC.Core.Game;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Entities;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.FormBattleship
{
    /// <summary>
    /// A Match between a User and a AI.
    /// </summary>
    public class UserMatch : MatchCore
    {

        public UserMatch(Configuration config) 
        {
            GameSize = 10;
            Turns = 0;
            CurrentPhase = Phase.Placement;

            Player PlayerUser = new Player(this, "User");
            PlayerAdd(PlayerUser);

        }

        public UserMatch()
            : this(Configuration.Global)
        {
        }

        /// <summary>
        /// Defines all of the states that are possible within the match.
        /// </summary>
        public enum Phase 
        {
            Placement, 
            AITurn, 
            UserTurn, 
            End 
        }

        /// <summary>
        /// Current phase of the match.
        /// </summary>
        public Phase CurrentPhase 
        { 
            get; 
            protected set;
        }
        
        public int Turns
        {
            get;
            protected set;
        }

        public override bool Play()
        {
            switch (CurrentPhase)
            {
                case Phase.AITurn:
                    //TODO: Computer Turn

                    CurrentPhase = Phase.UserTurn;
                    break;
                case Phase.End:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the game size
        /// </summary>
        public int GameSize
        {
            get;
            protected set;
        }

        
        public bool UserShoot(Shot asd)
        {
            if (CurrentPhase == Phase.UserTurn)
            {
                //TODO Player Shoot
                Turns++;
                CurrentPhase = Phase.AITurn;
                return true;
            }
            return false;
        }

        public bool UserSetShip(Coordinates coord, ShipOrientation orient, Ship ship)
        {
            if (CurrentPhase == Phase.Placement)
            {
                //TODO place ship
                //TODO if all placed startgame
                return true;
            }
            return false;
        }
        
    }
}
