using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.FormBattleship.Controls
{
    public static class FieldResources
    {
        private static Image hitImage;
        private static Image missImage;
        private static Image waterImage;
        private static Image sunkImage;
        private static Image[] carrierImages;
        private static Image carrierImage;
        private static Image[] battleshipImages;
        private static Image battleshipImage;
        private static Image[] cruiserImages;
        private static Image cruiserImage;
        private static Image[] destroyerImages;
        private static Image destroyerImage;
        private static Image[] subImages;
        private static Image subImage;

        static FieldResources()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            hitImage = LoadImage(assembly, "Missile_Hit.png");
            missImage = LoadImage(assembly, "Missile_Miss.png");
            sunkImage = LoadImage(assembly, "Missile_Sunk.png");
            waterImage = LoadImage(assembly, "Water.png");
            carrierImage = LoadImage(assembly, "Carrier.png");
            battleshipImage = LoadImage(assembly, "Battleship.png");
            cruiserImage = LoadImage(assembly, "Cruiser.png");
            destroyerImage = LoadImage(assembly, "Destroyer.png");
            subImage = LoadImage(assembly, "Sub.png");

            carrierImages = new Image[5];
            battleshipImages = new Image[4];
            cruiserImages = new Image[3];
            subImages = new Image[3];
            destroyerImages = new Image[2];
            for (int i = 0; i < 5; i++)
            {
                carrierImages[i] = LoadImage(assembly, string.Format("Carrier{0}.png", i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                battleshipImages[i] = LoadImage(assembly, string.Format("Battleship{0}.png", i + 1));
            }
            for (int i = 0; i < 3; i++)
            {
                cruiserImages[i] = LoadImage(assembly, string.Format("Cruiser{0}.png", i + 1));
                subImages[i] = LoadImage(assembly, string.Format("Sub{0}.png", i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                destroyerImages[i] = LoadImage(assembly, string.Format("Destroyer{0}.png", i + 1));
            }
        }

        private static Image LoadImage(Assembly assembly, string img)
        {
            Stream imageStream = assembly.GetManifestResourceStream("MBC.App.FormBattleship.resources." + img);
            return Image.FromStream(imageStream);
        }

        public static Image HitImage
        {
            get { return hitImage; }
        }

        public static Image MissImage
        {
            get { return missImage; }
        }

        public static Image WaterImage
        {
            get { return waterImage; }
        }

        public static Image[] CarrierImages
        {
            get { return carrierImages; }
        }

        public static Image[] BattleshipImages
        {
            get { return battleshipImages; }
        }

        public static Image[] CruiserImages
        {
            get { return cruiserImages; }
        }

        public static Image[] DestroyerImages
        {
            get { return destroyerImages; }
        }

        public static Image[] SubImages
        {
            get { return subImages; }
        }

        public static Image CarrierImage
        {
            get { return carrierImage; }
        }

        public static Image BattleshipImage
        {
            get { return battleshipImage; }
        }

        public static Image CruiserImage
        {
            get { return cruiserImage; }
        }

        public static Image DestroyerImage
        {
            get { return destroyerImage; }
        }

        public static Image SubImage
        {
            get { return subImage; }
        }

        public static Image SunkImage
        {
            get { return sunkImage; }
        }
    }
}
