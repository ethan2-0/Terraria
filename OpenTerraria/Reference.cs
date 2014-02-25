﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace OpenTerraria {
    public class Reference {
        public static String executablePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
        public static String imagePath = executablePath + "images\\";
        public static String version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //The following is how you'd get an image:
        //public static Bitmap grass = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("grass.png"));
        public static Bitmap getImage(String filename) {
            String path = imagePath + filename;
            return new Bitmap(path);
        }
    }
}
