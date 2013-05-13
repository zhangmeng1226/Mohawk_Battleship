using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IrrlichtLime;
using IrrlichtLime.Core;
using IrrlichtLime.GUI;
using IrrlichtLime.IO;
using IrrlichtLime.Scene;
using IrrlichtLime.Video;

using MBC.Core;

namespace MBC.App3D
{
    class Program
    {
        static void Main(string[] args)
        {
            IrrlichtDevice device = IrrlichtDevice.CreateDevice(DriverType.Software,
                new Dimension2Di(800, 600), 32, false, false, true);

            VideoDriver driver = device.VideoDriver;
            SceneManager mgr = device.SceneManager;

            AnimatedMesh mesh = mgr.GetMesh(Util.WorkingDirectory() + "models\\cube.3ds");
            if (mesh == null)
                Environment.Exit(254);
            AnimatedMeshSceneNode meshNode = mgr.AddAnimatedMeshSceneNode(mesh);
            meshNode.SetMaterialFlag(MaterialFlag.Lighting, false);

            mgr.AddCameraSceneNode(mgr.RootNode, new Vector3Df(0, 30, -40), new Vector3Df(0, 5, 0));

            while (device.Run())
            {
                driver.BeginScene(true, true, Color.OpaqueWhite);

                mgr.DrawAll();

                driver.EndScene();
            }
        }
    }
}
