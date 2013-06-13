using MBC.Core.Attributes;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MBC.Core
{
    /// <summary>
    /// The Controller class manages the controller Type objects which are classes that implement the
    /// IBattleshipController interface. Statically, it handles loading these classes and provides the name
    /// and version of these classes through a list of generated controller classes that are used to
    /// determine the available controllers.<br/>
    /// Use the following methods in this order to load and retrieve information for controllers:
    /// <list type="number">
    ///     <item>LoadControllerFolder() or AddControllerFolder() - Populates the list of ClassInfo objects that contain the Type objects required to create controller interfaces.</item>
    ///     <item>GetControllerList() - Gets the list of ClassInfo objects that have been constructed.</item>
    ///     <item>GetControllersFromName() - Gets a list of ClassInfo objects from a name.</item>
    ///     <item>GetController() - Gets a ClassInfo object from a name and version number.</item>
    /// </list>
    /// Configuration keys:
    /// <list type="bullet">
    ///     <item><b>mbc_controller_thread_timeout</b> - The time to wait before aborting a called method in a controller interface.
    ///     This value must be greater than the mbc_match_timeout value.
    ///     </item>
    /// </list>
    /// </summary>
    /// <seealso cref="ClassInfo"/>
    public partial class Controller
    {
        //This part of the Controller class deals with loading controllers from DLLs.

        /// <summary>Sets default configuration values for keys that relate to this class.
        /// Should be called before using the global Configuration.Default object.</summary>
        /// <seealso cref="Configuration"/>
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<int>("mbc_controller_thread_timeout", 1000);
        }

        private static Dictionary<NameVersionPair, ClassInfo> loadedControllerClasses;

        /// <summary>
        /// Loads all of the .dll files located in a given directory and stores information about all 
        /// of the controller classes implementing the IBattleshipController interface for future use.
        /// </summary>
        /// <param name="path">The absolute path name to a folder containing DLL files.</param>
        /// <exception cref="DirectoryNotFoundException">The given directory was not found or was a relative path.</exception>
        public static void AddControllerFolder(string path)
        {
            //filePaths should be a list of absolute paths to .dll files
            var filePaths = new List<string>(Directory.GetFiles(path, "*.dll"));

            foreach (var file in filePaths)
            {
                try
                {
                    var dllInfo = Assembly.LoadFile(file);
                    var types = dllInfo.GetTypes();

                    foreach (Type cont in types)
                    {
                        //Iterating through each class in this assembly.
                        if (cont.GetInterface("MBC.Core.IBattleshipController") != null)
                        {
                            NameAttribute nameAttrib = (NameAttribute)cont.GetCustomAttributes(typeof(NameAttribute), false)[0];
                            VersionAttribute verAttrib = (VersionAttribute)cont.GetCustomAttributes(typeof(VersionAttribute), false)[0];
                            if (nameAttrib != null && verAttrib != null)
                            {
                                //Split the absolute path. We only want the name of the DLL file.
                                string[] pathSplit = file.Split('\\');

                                ClassInfo info = new ClassInfo(nameAttrib, verAttrib,
                                    (DescriptionAttribute)cont.GetCustomAttributes(typeof(DescriptionAttribute), false)[0],
                                    (AuthorAttribute)cont.GetCustomAttributes(typeof(AuthorAttribute), false)[0],
                                    (AcademicInfoAttribute)cont.GetCustomAttributes(typeof(AcademicInfoAttribute), false)[0],
                                    (CapabilitiesAttribute)cont.GetCustomAttributes(typeof(CapabilitiesAttribute), false)[0],
                                    pathSplit[pathSplit.Count() - 1],
                                    cont);
                                loadedControllerClasses[new NameVersionPair(nameAttrib.Name, verAttrib.Version)] = info;
                            }
                            break;
                        }
                    }
                }
                catch
                {
                    //Unable to load a .DLL file; we don't care about this assembly.
                }
            }
        }

        /// <summary>
        /// Clears the currently loaded controllers from the internal list, loads all of the .dll files
        /// located in a given directory, and stores information about all of the controller classes
        /// implementing the IBattleshipController interface for future use.
        /// </summary>
        /// <param name="path">The absolute path name to a folder containing DLL files.</param>
        /// <exception cref="DirectoryNotFoundException">The given directory was not found or was a relative path.</exception>
        public static void LoadControllerFolder(string path)
        {
            loadedControllerClasses = new Dictionary<NameVersionPair, ClassInfo>();

            AddControllerFolder(path);
        }

        /// <summary>
        /// Retrieves a list of ClassInfo objects that represent the controller classes that have been loaded
        /// from DLL files.
        /// </summary>
        /// <returns>A List of ClassInfo objects.</returns>
        public static List<ClassInfo> GetControllerList()
        {
            return loadedControllerClasses.Values.ToList();
        }

        /// <summary>
        /// Retrieves a list of ClassInfo objects with the same name given. The only difference between
        /// controllers with identical names is the version number.
        /// </summary>
        /// <param name="name">The name of a controller.</param>
        /// <returns>A List of ClassInfo objects that have the same name given.</returns>
        public static List<ClassInfo> GetControllersFromName(string name)
        {
            var controllers = new List<ClassInfo>();
            foreach (var info in loadedControllerClasses.ToList())
            {
                if (info.Key.Name == name)
                {
                    controllers.Add(info.Value);
                }
            }
            return controllers;
        }

        /// <summary>
        /// Gets a ClassInfo object from the given name and version parameters. This returns null if no such
        /// controller with the given information exists.
        /// </summary>
        /// <param name="name">A string of the name of the controller to get.</param>
        /// <param name="ver">The Version of the controller to get.</param>
        /// <returns>A ClassInfo object of the resulting controller. null if there was no such controller
        /// found.</returns>
        public static ClassInfo GetController(string name, Version ver)
        {
            ClassInfo result;
            loadedControllerClasses.TryGetValue(new NameVersionPair(name, ver), out result);
            return result;
        }

        /// <summary>
        /// Provides information about a controller interface that has been loaded from a DLL file.
        /// </summary>
        public class ClassInfo
        {
            //The attributes loaded from the controller class.
            private NameAttribute nameAttrib;
            private VersionAttribute verAttrib;
            private DescriptionAttribute descAttrib;
            private AuthorAttribute authorAttrib;
            private AcademicInfoAttribute academicAttrib;
            private CapabilitiesAttribute capableAttrib;

            private string dllFile;
            private Type typeInterface;

            /// <summary>
            /// Constructs a ClassInfo object with the given values.
            /// </summary>
            /// <param name="name">The name of the controller.</param>
            /// <param name="ver">The version number of the controller.</param>
            /// <param name="dll">The file name of the DLL that this controller was loaded in.</param>
            /// <param name="inter">The class implementing the IBattleshipInterface (un-constructed state).</param>
            public ClassInfo(NameAttribute name, VersionAttribute ver, DescriptionAttribute desc,
                AuthorAttribute auth, AcademicInfoAttribute academic, CapabilitiesAttribute capabilities, 
                string dll, Type inter)
            {
                this.nameAttrib = name;
                this.verAttrib = ver;
                this.descAttrib = desc;
                this.authorAttrib = auth;
                this.academicAttrib = academic;
                this.capableAttrib = capabilities;
                this.dllFile = dll;
                this.typeInterface = inter;
            }

            /// <summary>
            /// Gets the name of this controller.
            /// </summary>
            public string Name
            {
                get
                {
                    return nameAttrib.Name;
                }
            }

            /// <summary>
            /// Gets the version number of this controller.
            /// </summary>
            public Version Version
            {
                get
                {
                    return verAttrib.Version;
                }
            }

            /// <summary>
            /// Gets a string describing this controller.
            /// </summary>
            public string Description
            {
                get
                {
                    return descAttrib.Description;
                }
            }

            /// <summary>
            /// Gets an AuthorAttribute class that contains information about the author.
            /// </summary>
            /// <seealso cref="AuthorAttribute"/>
            public AuthorAttribute AuthorInfo
            {
                get
                {
                    return authorAttrib;
                }
            }

            public List<GameMode> CompatibleModes
            {
                get
                {
                    return capableAttrib.Capabilities;
                }
            }

            /// <summary>
            /// Gets an AcademicInfoAttribute class that contains information about the author's academics.
            /// </summary>
            /// <seealso cref="AcademicInfoAttribute"/>
            public AcademicInfoAttribute AcademicInfo
            {
                get
                {
                    return academicAttrib;
                }
            }

            /// <summary>
            /// Gets the file name of the dll that this controller originates from.
            /// </summary>
            public string DLLFileName
            {
                get
                {
                    return dllFile;
                }
            }

            /// <summary>
            /// Gets the type class for this controller.
            /// </summary>
            public Type Controller
            {
                get
                {
                    return typeInterface;
                }
            }

            /// <summary>
            /// Generates and returns a string representation for this controller.
            /// </summary>
            public override string ToString()
            {
                return Name + " v(" + Version.ToString() + ")";
            }
        }

        /// <summary>
        /// A NameVersionPair object identifies a unique controller type. There can exist controllers
        /// with the same name or with the same version, but not both. NameVersionPair objects
        /// are used as keys for the internally used dictionary to store Controller ClassInfo objects.
        /// </summary>
        private class NameVersionPair : IEquatable<NameVersionPair>
        {
            private string name;
            private Version version;

            /// <summary>
            /// Initializes this NameVersionPair with the given values.
            /// </summary>
            /// <param name="name">The name of the controller.</param>
            /// <param name="version">The version number of the controller.</param>
            public NameVersionPair(string name, Version version)
            {
                this.name = name;
                this.version = version;
            }

            /// <summary>
            /// Gets the string representation of the name of a controller.
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// Gets the Version object that identifies the version associated with this controller.
            /// </summary>
            public Version Version
            {
                get
                {
                    return version;
                }
            }

            /// <summary>
            /// Returns a value that indicates whether or not this NameVersionPair is equal to another given
            /// NameVersionPair.
            /// </summary>
            /// <param name="obj">The NameVersionPair to compare to.</param>
            /// <returns>true if the values of this NameVersionPair is the same as the other. false otherwise.</returns>
            public bool Equals(NameVersionPair obj)
            {
                return obj.name == name && version.Equals(obj.version);
            }
        }
    }

    /// <summary>
    /// The Controller class also represents a loaded controller implementing the IBattleshipController class
    /// and wraps this class to provide various utilities such as timing.
    /// </summary>
    public partial class Controller
    {
        //This part of the Controller class deals with interface-wrapping Controller objects

        private IBattleshipController controllerInterface;
        private ClassInfo controllerInfo;
        private NameVersionPair identifier;

        private MatchInfo matchInfo;

        private int currentScore;

        private ShipList ships;
        private ShotList shotsMade;

        private ControllerID idNum;

        private Stopwatch timeElapsed;

        /// <summary>
        /// Invoked whenever the underlying controller interface wants to output a message string.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        /// <summary>
        /// Constructs a new Controller object with the given ClassInfo controller information and MatchInfo.
        /// </summary>
        /// <param name="targetControllerInfo">The ClassInfo to create the object from.</param>
        /// <param name="matchInfo">The MatchInfo to set the behaviour of this Controller for.</param>
        public Controller(ClassInfo targetControllerInfo, MatchInfo matchInfo)
        {
            this.matchInfo = matchInfo;
            this.controllerInfo = targetControllerInfo;

            controllerInterface = (IBattleshipController)Activator.CreateInstance(targetControllerInfo.Controller);
            controllerInterface.ControllerMessageEvent += ReceiveMessage;
            identifier = new NameVersionPair(controllerInfo.Name, controllerInfo.Version);
        }

        /// <summary>
        /// Constructs a Controller object without a controller interface to control. In this state, this object
        /// may only be used to provide the score, ships placed, and shots made. Used in reloading Match objects
        /// from files where the controller interface has been previously removed.
        /// </summary>
        public Controller(string name, Version ver, MatchInfo matchInfo)
        {
            this.matchInfo = matchInfo;

            identifier = new NameVersionPair(name, ver);
        }

        /// <summary>
        /// Invoked when the controller interface wants to output a string.
        /// </summary>
        /// <param name="message">A string containing the message.</param>
        private void ReceiveMessage(string message)
        {
            if (ControllerMessageEvent != null)
            {
                ControllerMessageEvent(message);
            }
        }

        /// <summary>
        /// Gets the ClassInfo that this Controller object refers to.
        /// </summary>
        /// <seealso cref="ClassInfo"/>
        public ClassInfo Info
        {
            get
            {
                return controllerInfo;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this Controller wraps around a controller interface.
        /// </summary>
        public bool InterfaceExists
        {
            get
            {
                return controllerInterface != null;
            }
        }

        /// <summary>
        /// Gets the time (in milliseconds) that the controller interface took to finish the last method invoked.
        /// </summary>
        public int TimeElapsed
        {
            get
            {
                return (int)timeElapsed.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Gets the name of the controller interface.
        /// </summary>
        public string Name
        {
            get
            {
                return identifier.Name;
            }
        }

        /// <summary>
        /// Gets the Version number of the controller interface.
        /// </summary>
        public Version Version
        {
            get
            {
                return identifier.Version;
            }
        }

        /// <summary>
        /// Gets a string representation of this Controller that may be used for display.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return Name+" v("+Version.ToString()+")";
            }
        }

        public ShipList Ships
        {
            get
            {
                return ships;
            }
        }

        public ShotList Shots
        {
            get
            {
                return shotsMade;
            }
        }

        public ControllerID ControllerID
        {
            get
            {
                return idNum;
            }
        }

        public MatchInfo MatchInfo
        {
            get
            {
                return matchInfo;
            }
        }

        /// <summary>
        /// Handles a given Thread by providing a timing and timeout function. If there is no controller interface
        /// loaded with this controller, the thread will not be called.
        /// </summary>
        /// <param name="thread">The Thread to monitor and time.</param>
        private void HandleThread(Thread thread, string method)
        {
            //Start the thread.
            timeElapsed.Restart();
            thread.Start();
            if (!thread.Join(matchInfo.TimeLimit))
            {
                //Thread timed out.
                thread.Abort();
            }
            timeElapsed.Stop();
            if (TimedOut())
            {
                throw new ControllerTimeoutException(this, method, TimeElapsed);
            }
        }

        /// <summary>
        /// Determines whether or not this Controller had exceeded the allowed time limit during the last method
        /// invoke.
        /// </summary>
        /// <returns>true if this Controller passed the time limit, false otherwise.</returns>
        public bool TimedOut()
        {
            return TimeElapsed > matchInfo.TimeLimit;
        }

        /// <summary>
        /// Resets match-related information made by this Controller and invokes the NewMatch method on the
        /// controller interface. Copies the information given instead of passing them to the controller interface
        /// by reference.
        /// </summary>
        /// <param name="opponentName">A string representing the name of the newly matched up opponent controller.</param>
        /// <param name="fieldSize">The size of the battlefield.</param>
        /// <param name="methodTime">The time limit given for this Controller.</param>
        /// <param name="initShips">The ships that a match will play with.</param>
        public void NewMatch()
        {
            currentScore = 0;

            var thread = new Thread(() =>
            controllerInterface.NewMatch(matchInfo));

            HandleThread(thread, "NewMatch");
        }

        /// <summary>
        /// Resets round-related information made by this Controller and invokes the NewRound method on the
        /// controller interface.
        /// </summary>
        public void NewRound(ControllerID id)
        {
            idNum = id;
            shotsMade = new ShotList();

            var thread = new Thread(() => controllerInterface.NewRound());

            HandleThread(thread, "NewRound");
        }

        /// <summary>
        /// Copies the initial ShipList from the match info and passes this list to the
        /// method PlaceShips in the controller interface to be invoked.
        /// </summary>
        public void PlaceShips()
        {
            ships = matchInfo.StartingShips;

            var thread = new Thread(() => controllerInterface.PlaceShips(ships.ToReadOnlyCollection()));

            HandleThread(thread, "PlaceShips");
        }

        /// <summary>
        /// Invokes the controller interface's GetShot method to get the Coordinates of its next shot.
        /// </summary>
        /// <returns>A Coordinates object of the controller's shot. If null, the controller interface failed to
        /// return the Coordinates within the time limit.</returns>
        public Shot MakeShot(ControllerID defaultReceiver)
        {
            Shot result = new Shot(defaultReceiver);
            var thread = new Thread(() => controllerInterface.MakeShot(result));

            HandleThread(thread, "GetShot");

            if (result == null)
            {
                result = new Shot(defaultReceiver);
            }

            return result;
        }

        /// <summary>
        /// Notifies this controller interface about a shot made by the opposing controller. Invokes the
        /// OpponentShot method with the Coordinates of the shot made.
        /// </summary>
        /// <param name="opShot">The Coordinates of the opposing controller's shot.</param>
        public void NotifyOpponentShot(Shot opShot)
        {
            var thread = new Thread(() => controllerInterface.OpponentShot(opShot));

            HandleThread(thread, "OpponentShot");
        }

        /// <summary>
        /// Notifies this controller about a shot that was previously made that had shot an opposing controller's
        /// Ship with an indication of whether or not the shot sunk the ship. Invokes the ShotHit method
        /// of the controller interface with this information.
        /// </summary>
        /// <param name="shotMade">The Coordinates of the shot previously made.</param>
        /// <param name="sink">true if the Coordinates of the shot sunk a ship, false otherwise.</param>
        public void NotifyShotHit(Shot shotMade, bool sink)
        {
            var thread = new Thread(() => controllerInterface.ShotHit(shotMade, sink));

            HandleThread(thread, "ShotHit");
        }

        /// <summary>
        /// Notifies this controller about a shot that has been previously made that had missed an opposing
        /// controller's Ship. Invokes the controller interface's ShotMiss method with the Coordinates of this
        /// Shot.
        /// </summary>
        /// <param name="shotMade">The Coordinates of the shot that missed.</param>
        public void NotifyShotMiss(Shot shotMade)
        {
            var thread = new Thread(() => controllerInterface.ShotMiss(shotMade));

            HandleThread(thread, "ShotMiss");
        }

        /// <summary>
        /// Adds to this Controller object's score and invokes the RoundWon method on the controller interface.
        /// </summary>
        public void RoundWon()
        {
            currentScore++;

            var thread = new Thread(() =>
            controllerInterface.RoundWon());

            HandleThread(thread, "RoundWon");
        }

        /// <summary>
        /// Invokes the RoundLost method on the controller interface.
        /// </summary>
        public void RoundLost()
        {
            var thread = new Thread(() =>
            controllerInterface.RoundLost());

            HandleThread(thread, "RoundLost");
        }

        /// <summary>
        /// Resets Match-specific variables to null values and invokes the controller interface's MatchOver
        /// method.
        /// </summary>
        public void MatchOver()
        {
            matchInfo = null;

            var thread = new Thread(() =>
            controllerInterface.MatchOver());

            HandleThread(thread, "MatchOver");
        }

        /// <summary>
        /// Gets a string representation of this Controller. This is the same as the DisplayName property.
        /// </summary>
        /// <returns>A string that names this Controller.</returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
