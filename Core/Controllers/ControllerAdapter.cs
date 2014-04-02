﻿using MBC.Shared;
using MBC.Shared.Attributes;
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
        private Player myPlayer;
        private IController oldController;

        public ControllerAdapter(IController oldController)
        {
            this.oldController = oldController;
        }

        public Player Player
        {
            get
            {
                return myPlayer;
            }
            set
            {
                myPlayer = value;
                Initialize();
            }
        }

        [EventFilter(typeof(MatchBeginEvent))]
        private void HandleMatchBegin(Event ev)
        {
            oldController.NewMatch();
        }

        [EventFilter(typeof(MatchEndEvent))]
        private void HandleMatchEnd(Event ev)
        {
            oldController.MatchOver();
        }

        [EventFilter(typeof(MatchAddPlayerEvent))]
        private void HandlePlayerAdd(Event ev)
        {
            MatchAddPlayerEvent evCasted = (MatchAddPlayerEvent)ev;
            Player plr = evCasted.Player;
            plr.OnEvent += HandlePlayerLose;
            plr.OnEvent += HandlePlayerShot;
            plr.OnEvent += HandlePlayerWin;
            if (plr == myPlayer)
            {
                oldController.ID = plr.ID;
            }
            oldController.Registers.Add(plr.ID, new Register(plr.ID, plr.Name));
        }

        [EventFilter(typeof(PlayerLostEvent))]
        private void HandlePlayerLose(Event ev)
        {
            Player plr = ((PlayerLostEvent)ev).Player;
            if (plr == myPlayer)
            {
                oldController.RoundLost();
            }
            else
            {
                oldController.OpponentDestroyed(plr.ID);
            }
        }

        [EventFilter(typeof(MatchRemovePlayerEvent))]
        private void HandlePlayerRemove(Event ev)
        {
            Player plr = ((PlayerLostEvent)ev).Player;
            plr.OnEvent -= HandlePlayerLose;
            plr.OnEvent -= HandlePlayerShot;
            plr.OnEvent -= HandlePlayerWin;
            oldController.Registers.Remove(((MatchRemovePlayerEvent)ev).Player.ID);
        }

        [EventFilter(typeof(PlayerShotEvent))]
        private void HandlePlayerShot(Event ev)
        {
            PlayerShotEvent evCasted = (PlayerShotEvent)ev;
            Player plr = evCasted.Player;
            Shot shot = evCasted.Shot;
            if (plr == myPlayer)
            {
                Ship shipHit = ShipList.GetShipAt(shot);
                if (shipHit == null)
                {
                    oldController.ShotMiss(shot);
                }
                else
                {
                    oldController.ShotHit(shot, shipHit.IsSunk());
                }
            }
            else
            {
                oldController.OpponentShot(shot);
            }
        }

        [EventFilter(typeof(PlayerWonEvent))]
        private void HandlePlayerWin(Event ev)
        {
            if (((PlayerWonEvent)ev).Player == myPlayer)
            {
                oldController.RoundWon();
            }
        }

        [EventFilter(typeof(RoundBeginEvent))]
        private void HandleRoundBegin(Event ev)
        {
            oldController.NewRound();
            foreach (Ship ship in oldController.PlaceShips())
            {
                Ship existingShip = myPlayer.Ships.Where(x => x.Length == ship.Length && !x.IsPlaced).First();
                if (existingShip != null)
                {
                    existingShip.Place(ship.Location, ship.Orientation);
                }
            }
        }

        [EventFilter(typeof(MatchTeamAddEvent))]
        private void HandleTeamAdd(Event ev)
        {
            Team team = ((MatchTeamAddEvent)ev).Team;
            oldController.Teams.Add(team.ID, team);
        }

        [EventFilter(typeof(MatchTeamRemoveEvent))]
        private void HandleTeamRemove(Event ev)
        {
            oldController.Teams.Remove(((MatchTeamRemoveEvent)ev).Team.ID);
        }

        private void Initialize()
        {
            Match containingMatch = myPlayer.Match;
            oldController.ControllerMessageEvent += (string msg) =>
            {
                myPlayer.Message(msg);
            };
            oldController.Field = new FieldInfo();

            oldController.Match = new MatchConfig(Player.Match);

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

            containingMatch.OnEvent += HandlePlayerAdd;
            containingMatch.OnEvent += HandlePlayerRemove;
            containingMatch.OnEvent += HandleTeamAdd;
            containingMatch.OnEvent += HandleTeamRemove;
            containingMatch.OnEvent += HandleMatchBegin;
            containingMatch.OnEvent += HandleMatchEnd;
            containingMatch.OnEvent += HandleRoundBegin;
        }
    }
}