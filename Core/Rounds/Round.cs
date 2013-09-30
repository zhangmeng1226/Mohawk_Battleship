using MBC.Core.Accolades;
using MBC.Core.Events;
using MBC.Core.Matches;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace MBC.Core.Rounds
{

    public abstract class Round
    {
        public event MBCEventHandler EventGenerated;

        private List<IController> controllers;

        public Round(MatchConfig roundParams)
        {

        }

        public void AddController(IController ctrl)
        {

        }

        public void RemoveController(IController ctrl)
        {

        }

        public abstract void Play();
    }
}