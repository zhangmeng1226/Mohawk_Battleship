using MBC.Shared;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Controllers
{
    /// <summary>
    /// Wraps an IController interface and adapts it to the new IController2 interface.
    /// </summary>
    internal class ControllerAdapter : IController2
    {
        private IController oldController;

        public ControllerAdapter(IController oldController)
        {
            this.oldController = oldController;
            oldController.ControllerMessageEvent += (string msg) =>
            {
                if (ControllerMessageEvent != null)
                {
                    ControllerMessageEvent(msg);
                }
            };
            Match = new Match();

            Match.OnMatchBegin += (object match, MatchBeginEvent ev) => { oldController.NewMatch(); };
            Match.OnMatchEnd += (object match, MatchEndEvent ev) => { oldController.MatchOver(); };
            Match.OnRoundBegin += (object match, RoundBeginEvent ev) => { oldController.NewRound(); };
            Match.OnPlayerLose += (object match, PlayerLostEvent ev) =>
                {
                    if (ev.Player.Controller == oldController)
                    {
                        oldController.RoundLost();
                    }
                    else
                    {
                        oldController.OpponentDestroyed(ev.Player.ID);
                    }
                };
            Match.OnPlayerShot += (object match, PlayerShotEvent ev) =>
                {
                    if (ev.Player.Controller == oldController)
                    {
                        if (ev.ShipHit == null)
                        {
                            oldController.ShotMiss(ev.Shot);
                        }
                        else
                        {
                            oldController.ShotHit(ev.Shot, ev.ShipHit.IsSunk());
                        }
                    }
                    else
                    {
                        oldController.OpponentShot(ev.Shot);
                    }
                };
            Match.OnPlayerWin += (object match, PlayerWonEvent ev) =>
                {
                    if (ev.Player.Controller == oldController)
                    {
                        oldController.RoundWon();
                    }
                };
        }

        public event StringOutputHandler ControllerMessageEvent;

        public Match Match
        {
            get;
            set;
        }

        public Shot MakeShot()
        {
            return oldController.MakeShot();
        }

        public IList<Ship> PlaceShips()
        {
            return oldController.PlaceShips().ToList();
        }
    }
}