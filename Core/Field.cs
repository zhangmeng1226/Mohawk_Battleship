using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MBC.Core
{

    
    /// <summary>The Field class represents a battleship field. It contains battleship field information and
    /// two ControllerInfo objects that contain information on the two controllers in the Field.
    /// 
    /// Accessing the individual ControllerInfo objects is made simple by using a value of 0 or 1. The order
    /// of the ControllerInfo objects does not change, so constants can be used to access them.</summary>
    [Serializable]
    public class Field
    {
        /// <summary>
        /// The size of the battlefield
        /// </summary>
        public Size gameSize;

        /// <summary>
        /// A Random object
        /// </summary>
        public Random fixedRandom;

        /// <summary>
        /// A list of all the ships available on the battlefield
        /// </summary>
        public List<int> shipSizes;

        /// <summary>
        /// The time limit for this field
        /// </summary>
        public TimeSpan timeoutLimit;
        private ControllerInfo[] info;  //A 2-element array that contains information for each controller.

        
        /// <summary>Constructs a Battlefield object initialized with two opponents</summary>
        public Field(IBattleshipController[] ibc)
        {
            info = new ControllerInfo[2];
            info[0] = new ControllerInfo(ibc[0].Name, ibc[0].Version);
            info[1] = new ControllerInfo(ibc[1].Name, ibc[1].Version);
        }

        
        /// <summary>Copy constructor</summary>
        public Field(Field copy)
        {
            info = (ControllerInfo[])copy.info.Clone();
            gameSize = copy.gameSize;
            fixedRandom = copy.fixedRandom;
            shipSizes = copy.shipSizes;
            timeoutLimit = copy.timeoutLimit;
        }

        
        /// <summary>Gets the controllers in this field.</summary>
        public ControllerInfo[] Controllers
        {
            get { return info; }
        }

        
        /// <summary>It is possible to treat this Field object as a sort of array, by providing the
        /// index value in square-brackets on the object itself.</summary>
        /// <returns>The ConrollerInfo object at the specified index.</returns>
        public ControllerInfo this[int i]
        {
            get { return info[i]; }
        }

        
        /// <summary>Contains information related to the state of the battlefield for
        /// each controller.</summary>
        [Serializable]
        public class ControllerInfo
        {
            /// <summary>
            /// The shots made by this controller.
            /// </summary>
            public List<Point> shotsMade;

            /// <summary>
            /// The ship placement of this controller.
            /// </summary>
            public List<Ship> ships;

            /// <summary>
            /// The score for this controller.
            /// </summary>
            public int score = 0;

            /// <summary>
            /// The name of this controller.
            /// </summary>
            public string name;

            /// <summary>
            /// The version of this controller.
            /// </summary>
            public Version version;

            /// <summary>
            /// Contructs a new ControllerInfo that contains information about a controller.
            /// </summary>
            /// <param name="ctrlName">The name of the controller.</param>
            /// <param name="ctrlVersion">The version of the controller.</param>
            public ControllerInfo(string ctrlName, Version ctrlVersion)
            {
                shotsMade = new List<Point>();
                ships = new List<Ship>();
                name = ctrlName;
                version = ctrlVersion;
            }
        }
    }
}
