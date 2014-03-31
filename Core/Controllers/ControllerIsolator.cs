using MBC.Core.Controllers;
using MBC.Shared;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MBC.Core.Game
{
    /// <summary>
    /// Acts as a buffer between the match core and the match client application domains, allowing for restrictions to be
    /// placed upon the 3rd-party code being executed.
    /// </summary>
    internal class ControllerIsolator : MarshalByRefObject
    {
        private AppDomain sandbox;

        public ControllerIsolator()
        {
        }

        public Match ClientMatch
        {
            get;
            set;
        }

        public IController2 Controller
        {
            get;
            private set;
        }

        public static ControllerIsolator Isolate(ControllerSkeleton skeleton, MatchCore originalMatch)
        {
            AppDomain sandbox = AppDomain.CreateDomain(Guid.NewGuid().ToString());
            ControllerIsolator isolator = (ControllerIsolator)sandbox.CreateInstanceAndUnwrap(Assembly.GetCallingAssembly().FullName, typeof(ControllerIsolator).FullName);
            isolator.Initialize(skeleton, originalMatch, sandbox);
            return isolator;
        }

        private void Initialize(ControllerSkeleton skeleton, MatchCore match, AppDomain sandbox)
        {
            ClientMatch = new Match();
            Controller = skeleton.CreateInstance();
            Controller.Match = ClientMatch;
            this.sandbox = sandbox;

            match.OnMatchBegin += OnMatchBegin;
            match.OnMatchEnd += OnMatchEnd;
            match.OnPlayerAdd += OnPlayerAdd;
            match.OnPlayerDisqualified += OnPlayerDisqualified;
            match.OnPlayerLose += OnPlayerLose;
            match.OnPlayerMessage += OnPlayerMessage;
            match.OnPlayerRemove += OnPlayerRemove;
            match.OnPlayerShipDestruction += OnPlayerShipDestruction;
            match.OnPlayerShipsPlaced += OnPlayerShipsPlaced;
            match.OnPlayerShot += OnPlayerShot;
            match.OnPlayerTeamAssign += OnPlayerTeamAssign;
            match.OnPlayerTimeout += OnPlayerTimeout;
            match.OnPlayerTurnSwitch += OnPlayerTurnSwitch;
            match.OnPlayerWin += OnPlayerWin;
            match.OnRoundBegin += OnRoundBegin;
            match.OnRoundEnd += OnRoundEnd;
            match.OnTeamAdd += OnTeamAdd;
            match.OnTeamRemove += OnTeamRemove;
        }

        private void OnMatchBegin(object sender, MatchBeginEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnMatchEnd(object sender, MatchEndEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerAdd(object sender, MatchAddPlayerEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerDisqualified(object sender, PlayerDisqualifiedEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerLose(object sender, PlayerLostEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerMessage(object sender, PlayerMessageEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerRemove(object sender, MatchRemovePlayerEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerShipDestruction(object sender, ShipDestroyedEvent e)
        {
            if (e.Player.Controller != Controller)
            {
                ClientMatch.NotifyEvent(new ShipDestroyedEvent(e.Player, null));
            }
            else
            {
                ClientMatch.NotifyEvent(e);
            }
        }

        private void OnPlayerShipsPlaced(object sender, ShipMovedEvent e)
        {
            if (e.Player.Controller != Controller)
            {
                ClientMatch.NotifyEvent(new ShipMovedEvent(e.Player, null));
            }
            else
            {
                ClientMatch.NotifyEvent(e);
            }
        }

        private void OnPlayerShot(object sender, PlayerShotEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerTeamAssign(object sender, PlayerTeamAssignEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerTimeout(object sender, PlayerTimeoutEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerTurnSwitch(object sender, PlayerTurnSwitchEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnPlayerWin(object sender, PlayerWonEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnRoundBegin(object sender, RoundBeginEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnRoundEnd(object sender, RoundEndEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnTeamAdd(object sender, MatchTeamAddEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }

        private void OnTeamRemove(object sender, MatchTeamRemoveEvent e)
        {
            ClientMatch.NotifyEvent(e);
        }
    }
}