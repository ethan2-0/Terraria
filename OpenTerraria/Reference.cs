using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Media;

namespace OpenTerraria {
    public class Reference {
        public static String executablePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
        public static String imagePath = executablePath + "images\\";
        public static String soundPath = executablePath + "sounds\\";
        public static String version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static Color guiColor = Color.FromArgb(220, 0, 0, 0);
        //The following is how you'd get an image:
        //public static Bitmap grass = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("grass.png"));
        public static Bitmap getImage(String filename) {
            String path = imagePath + filename;
            return new Bitmap(path);
        }
        public static void playSoundAsync(String filename) {
            SoundPlayer player = new SoundPlayer(soundPath + filename);
            player.Play();
        }
    }
}
