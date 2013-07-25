using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MBC.Core.Matches
{
    /// <summary>
    /// A <see cref="Match"/> that utilizes <see cref="ControllerUser"/>s to generate new <see cref="Event"/>s.
    /// </summary>
    public class ControlledMatch : Match
    {
        /// <summary>
        /// The <see cref="ControllerUser"/>s participating in this <see cref="Match"/>.
        /// </summary>
        private List<ControllerUser> controllers;

        /// <summary>
        /// Inits a <see cref="ControlledMatch"/> with the given <paramref name="conf"/> and sets up the <see cref="Register"/>s
        /// to the <paramref name="controllerinfoLst"/>.
        /// </summary>
        /// <param name="conf">The <see cref="Configuration"/> used to set up this <see cref="Match"/>.</param>
        /// <param name="controllerInfoLst">A collection of <see cref="ControllerInformation"/> that defines the
        /// <see cref="ControllerUser"/>s to generate.</param>
        public ControlledMatch(Configuration conf, IEnumerable<ControllerInformation> controllerInfoLst)
            : base(conf)
        {
            Init(controllerInfoLst);
        }

        /// <summary>
        /// Inits a <see cref="ControlledMatch"/> with the given <paramref name="conf"/> and sets up the <see cref="Register"/>s
        /// to the <paramref name="controllerinfoLst"/>.
        /// </summary>
        /// <param name="conf">The <see cref="Configuration"/> used to set up this <see cref="Match"/>.</param>
        /// <param name="controllerInfos">A collection of <see cref="ControllerInformation"/> that defines the
        /// <see cref="ControllerUser"/>s to generate.</param>
        public ControlledMatch(Configuration conf, params ControllerInformation[] controllerInfos)
            : base(conf)
        {
            Init(controllerInfos);
        }

        /// <summary>
        /// Private constructor used in serialization.
        /// </summary>
        private ControlledMatch() : base() { }

        /// <summary>
        /// Overrides the <see cref="Match.CreateNewRound()"/>, which creates and returns a new <see cref="ClassicRound"/>.
        /// </summary>
        /// <returns>A <see cref="ClassicRound"/>.</returns>
        internal override Round CreateNewRound()
        {
            Round newRound = null;
            if (Info.GameMode.HasFlag(GameMode.Classic))
            {
                newRound = new ClassicRound(Info, controllers);
            }
            return newRound;
        }

        /// <summary>
        /// Generates a list of strings that represent the display names of <paramref name="controllers"/>.
        /// </summary>
        /// <param name="controllers">A collection of <see cref="ControllerInformation"/> objects.</param>
        /// <returns>A collection of strings.</returns>
        private static IEnumerable<string> NamesFromInformation(IEnumerable<ControllerInformation> controllers)
        {
            foreach (var controller in controllers)
            {
                yield return controller.ToString();
            }
        }

        /// <summary>
        /// Creates the <see cref="Register"/>s from the <see cref="ControllerUser"/>s in this <see cref="ControlledMatch"/>.
        /// </summary>
        private void CreateRegisters()
        {
            registers = new List<Register>();
            var info = (CMatchInfo)Info;
            for (var id = 0; id < controllers.Count; id++)
            {
                Registers.Add(new Register(Info, id));
                info.AddControllerName(controllers[id].ToString());
            }
        }

        /// <summary>
        /// Creates the teams/opponents for each <see cref="Register"/>.
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

        /// <summary>
        /// Generates the <see cref="ConrollerUser"/>s for this <see cref="ControlledMatch"/> through a
        /// collection of <see cref="ControllerInformation"/>.
        /// </summary>
        /// <param name="ctrlInfos">The <see cref="ControllerInformation"/> objects that create the <see cref="ControllerUser"/>s.</param>
        private void GenerateControllers(IEnumerable<ControllerInformation> ctrlInfos)
        {
            controllers = new List<ControllerUser>();
            for (var id = 0; id < ctrlInfos.Count(); id++)
            {
                controllers.Add(new ControllerUser(ctrlInfos.ElementAt(id)));
            }
        }

        /// <summary>
        /// Initializes this <see cref="ControlledMatch"/>.
        /// </summary>
        /// <param name="controllerInfoLst">A collection of <see cref="ControllerInformation"/>.</param>
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