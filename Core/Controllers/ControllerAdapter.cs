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
        private Match containingMatch;
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
            oldController.Field = new FieldInfo();
        }

        public event StringOutputHandler ControllerMessageEvent;

        public Match Match
        {
            get
            {
                return containingMatch;
            }
            set
            {
                containingMatch = value;
                SetupMatch();
            }
        }

        public Shot MakeShot()
        {
            return oldController.MakeShot();
        }

        public IList<Ship> PlaceShips()
        {
            return oldController.PlaceShips().ToList();
        }

        public void SetupMatch()
        {
            oldController.Match = new MatchConfig(Match);

            oldController.Registers = new Dictionary<IDNumber, Register>();
            foreach (var player in containingMatch.Players)
            {
                oldController.Registers.Add(player.ID, new Register(player.ID, player.Name));
            }

            oldController.Teams = new Dictionary<IDNumber, Team>();
            foreach (var team in containingMatch.Teams)
            {
                oldController.Teams.Add(team.ID, team);
            }

            Match.OnPlayerAdd += (object match, MatchAddPlayerEvent ev) =>
                {
                    if (ev.Player.Controller == oldController)
                    {
                        oldController.ID = ev.Player.ID;
                    }
                    oldController.Registers.Add(ev.Player.ID, new Register(ev.Player.ID, ev.Player.Name));
                };
            Match.OnPlayerRemove += (object match, MatchRemovePlayerEvent ev) => { oldController.Registers.Remove(ev.Player.ID); };
            Match.OnTeamAdd += (object match, MatchTeamAddEvent ev) => { oldController.Teams.Add(ev.Team.ID, ev.Team); };
            Match.OnTeamRemove += (object match, MatchTeamRemoveEvent ev) => { oldController.Teams.Remove(ev.Team.ID); };
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
    }
}