using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IrrlichtLime;
using IrrlichtLime.Core;
using IrrlichtLime.Video;
using IrrlichtLime.Scene;
using IrrlichtLime.GUI;
using System.IO;

namespace Battleship
{
    /**
     * <summary>BattleshipConfig deals with a single configuration file and parses each key/value pair</summary>
     */
    public class BattleshipConfig
    {
        //GUI stuff
        private const int GUI_BTN_Ok = 0; //"OK" button ID
        private const int GUI_BTN_Exit = 1; //"Cancel" button ID
        private bool guiIsRunning = false;
        private GUIListBox listBox; //The list box containing the rendering options

        //The rest
        private Dictionary<string, string> simpleConfig;
        private string absPath;

        /**
         * <summary>Initializes this BattleshipConfig by loading from a config file, if it exists</summary>
         */
        public BattleshipConfig(string pathname)
        {
            simpleConfig = new Dictionary<string, string>();
            absPath = pathname;
            LoadConfigFile();
        }

        /**
         * <summary>Gets a value from the configuration</summary>
         * <param name="s">The key to get the value from</param>
         * <param name="def">The default value for a given key if the key is not found.</param>
         * <exception cref="Exception">If the value could not be converted</exception>
         */
        private T ConfigValue<T>(string s, T def)
        {
            string valString;
            try
            {
                if (simpleConfig.TryGetValue(s, out valString))
                    return (T)Convert.ChangeType(valString, typeof(T));
            }
            catch { }
            simpleConfig[s] = def.ToString();
            return def;
        }

        /**
         * <summary>Event function for the GUI</summary>
         */
        private bool ConfigChooserEvent(Event e)
        {
            if (e.Type != EventType.GUI) return false;
            if (e.GUI.Type != GUIEventType.ButtonClicked) return false;
            switch (e.GUI.Caller.ID)
            {
                case 0:
                    switch (listBox.SelectedItem)
                    {
                        case "OpenGL":
                            simpleConfig["VIDEO_RENDERER"] = "opengl";
                            break;
                        case "DirectX 8.1":
                            simpleConfig["VIDEO_RENDERER"] = "dx8";
                            break;
                        case "DirectX 9":
                            simpleConfig["VIDEO_RENDERER"] = "dx9";
                            break;
                        case "Software":
                            simpleConfig["VIDEO_RENDERER"] = "software";
                            break;
                    }
                    guiIsRunning = false;
                    break;
                case 1:
                    Environment.Exit(0);
                    break;
            }
            return false;
        }

        /**
         * <summary>Brings up a GUI for the user, to select a rendering system to use.</summary>
         * <exception cref="NullPointerException">If software rendering won't work (something very wrong!)</exception>
         */
        private void RequestRenderSystem()
        {
            IrrlichtDevice dev = IrrlichtDevice.CreateDevice(DriverType.Software,
                new Dimension2Di(420, 120));
            VideoDriver driver = dev.VideoDriver;
            GUIEnvironment env = dev.GUIEnvironment;

            dev.SetWindowCaption("Select a rendering device.");
            dev.SetWindowResizable(false);

            env.AddStaticText("Select a rendering method:", new Recti(20, 20, driver.ViewPort.Width, driver.ViewPort.Height));

            listBox = env.AddListBox(new Recti(
                driver.ViewPort.Width / 2 - 120,
                40,
                driver.ViewPort.Width / 2 + 120,
                driver.ViewPort.Height - 30));
            listBox.AddItem("OpenGL");
            listBox.AddItem("DirectX 8.1");
            listBox.AddItem("DirectX 9");
            listBox.AddItem("Software");


            env.AddButton(new Recti(
                driver.ViewPort.Width / 2 - 25 - 30,
                driver.ViewPort.Height - 25,
                driver.ViewPort.Width / 2 + 25 - 30,
                driver.ViewPort.Height), null, GUI_BTN_Ok, "Ok", "");

            env.AddButton(new Recti(
                driver.ViewPort.Width / 2 - 25 + 30,
                driver.ViewPort.Height - 25,
                driver.ViewPort.Width / 2 + 25 + 30,
                driver.ViewPort.Height), null, GUI_BTN_Exit, "Cancel", "");

            dev.OnEvent += new IrrlichtDevice.EventHandler(ConfigChooserEvent);

            guiIsRunning = true;
            while (guiIsRunning && dev.Run())
                if (dev.WindowActive)
                {
                    driver.BeginScene(true, true, new Color(200, 200, 200));
                    env.DrawAll();
                    driver.EndScene();
                }
            dev.Drop();
        }

        /**
         * <summary>Loads a delimited configuration file, delimited by '=', and stores each key and value.</summary>
         */
        private void LoadConfigFile()
        {
            try
            {
                StreamReader reader = new StreamReader(absPath);
                do
                {
                    string line = reader.ReadLine();
                    string[] tLine = line.Split('=');
                    tLine[0] = tLine[0].Trim();
                    tLine[1] = tLine[1].Trim();
                    Console.WriteLine("Key: \"" + tLine[0] + "\"   Value: \"" + tLine[1] + "\"");
                    simpleConfig.Add(tLine[0], tLine[1]);
                } while (reader.Peek() != -1);
                reader.Close();
            }
            catch
            {
                Console.WriteLine("Could not load configuration file for reading.");
            }
        }

        /**
         * <summary>Saves the current configuration to a file.</summary>
         * <exception cref="IOException">The file could not be written to.</exception>
         */
        public void SaveConfigFile()
        {
            StreamWriter writer = new StreamWriter(absPath, false);
            try
            {
                foreach (KeyValuePair<string, string> entry in simpleConfig)
                    writer.WriteLine(entry.Key + " = " + entry.Value);
                writer.Flush();
                writer.Close();
            }
            catch
            {
                Console.WriteLine("Could not open the configuration file for writing.");
            }
        }

        /**
         * <summary>Returns an IrrlichtDevice ready for rendering based on the VIDEO_RENDERER key</summary>
         * <exception cref="NullPointerException">The software renderer won't load (something going bad!)</exception>
         */
        public IrrlichtDevice GetConfiguredDevice()
        {
            string deviceStr;
            if (!simpleConfig.TryGetValue("VIDEO_RENDERER", out deviceStr))
                RequestRenderSystem();

            DriverType driver = DriverType.Null;
            switch (deviceStr)
            {
                case "opengl":
                    driver = DriverType.OpenGL;
                    break;
                case "dx8":
                    driver = DriverType.Direct3D8;
                    break;
                case "dx9":
                    driver = DriverType.Direct3D9;
                    break;
                case "software":
                    driver = DriverType.Software;
                    break;
            }
            IrrlichtDevice nulldevice = IrrlichtDevice.CreateDevice(DriverType.Null);

            int width, height, bits;
            bool fullscreen, vsync;
            Dimension2Di resolution = nulldevice.VideoModeList.Desktop.Resolution;

            width = ConfigValue<int>("VIDEO_WIDTH", resolution.Width);
            height = ConfigValue<int>("VIDEO_HEIGHT", resolution.Height);
            bits = ConfigValue<int>("VIDEO_COLOR_DEPTH", nulldevice.VideoModeList.Desktop.Depth);
            fullscreen = ConfigValue<bool>("VIDEO_FULLSCREEN", true);
            vsync = ConfigValue<bool>("VIDEO_VSYNC", true);

            nulldevice.Drop();

            return IrrlichtDevice.CreateDevice(driver, new Dimension2Di(width, height), bits, fullscreen, true, vsync);
        }
    }
}
