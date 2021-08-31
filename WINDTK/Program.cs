using System;
using System.Collections.Generic;
using WINDTK.Types;

namespace WINDTK
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            WXNFile file = new WXNFile();

            file.ClearWriteMemory();

            file.WritePure(new WXNPureObject("Version", "69.1"));
            file.WritePure(new WXNPureObject("Rênân", "V0.0.0.0.0.0.0.0.0.1.2"));
            file.WritePure(new WXNPureObject("Arajo", "V-1"));

            file.Write(new WXNObject(WXNTypes.String, "Professor", "Ramão"));
            file.Write(new WXNObject(WXNTypes.String, "Professor", "Serjo"));
            file.Write(new WXNObject(WXNTypes.String, "Professor", "Dego"));

            file.Write(new WXNObject(WXNTypes.Int, "Número Legal KKK", 6598437));

            file.Save("Dingledong.wxn");

            WXNFileContent dingledong = file.Read("Dingledong.wxn");

            Console.WriteLine(dingledong.ToString());
        }
    }
}
