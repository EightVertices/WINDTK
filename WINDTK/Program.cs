using System;
using System.Collections.Generic;

namespace WINDXN
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //using (var Application = new Engine()) { Application.Run(); }

            Classes.Scene scene = new Classes.Scene("Blammed");
            scene.Initialize();
        }
    }
}
