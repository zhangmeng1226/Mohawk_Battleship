using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MBC.Core.Matches
{
    public class ControlledMatch : Match
    {
        private List<ControllerUser> controllers;

        public ControlledMatch(Configuration conf, IEnumerable<ControllerInformation> controllerInfoLst)
            : base(conf)
        {
            Init(controllerInfoLst);
        }

        public ControlledMatch(Configuration conf, params ControllerInformation[] controllerInfos)
            : base(conf)
        {
            Init(controllerInfos);
        }

        internal override Round CreateNewRound()
        {
            Round newRound = null;
            if (Info.GameMode.HasFlag(GameMode.Classic))
            {
                newRound = new ClassicRound(Info, controllers);
            }
            return newRound;
        }

        private static IEnumerable<string> NamesFromInformation(IEnumerable<ControllerInformation> controllers)
        {
            foreach (var controller in controllers)
            {
                yield return controller.ToString();
            }
        }

        private void CreateRegisters()
        {
            Registers = new List<Register>();
            var info = (CMatchInfo)Info;
            for (var id = 0; id < controllers.Count; id++)
            {
                Registers.Add(new Register(Info, id));
                info.AddControllerName(controllers[id].ToString());
            }
        }

        /// <summary>
        /// No teams taken into account.
        /// </summary>
        private void FormOpponents()
        {
            foreach (var registrant in Registers)
            {
                registrant.Opponents = new List<ControllerID>();
                foreach (var otherRegister in Registers)
                {
                    if (otherRegister == registrant) continue;
                    registrant.Opponents.Add(otherRegister.ID);
                }
            }
        }

        private void GenerateControllers(IEnumerable<ControllerInformation> ctrlInfos)
        {
            controllers = new List<ControllerUser>();
            for (var id = 0; id < ctrlInfos.Count(); id++)
            {
                controllers.Add(new ControllerUser(ctrlInfos.ElementAt(id)));
            }
        }

        private void Init(IEnumerable<ControllerInformation> controllerInfoLst)
        {
            GenerateControllers(controllerInfoLst);
            CreateRegisters();
            FormOpponents();
            foreach (var register in Registers)
            {
                controllers[register.ID].NewMatch(register);
            }

            Rounds.NextRound();
        }
    }
}