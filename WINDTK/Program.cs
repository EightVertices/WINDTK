using System;
using System.IO;

namespace WINDTK
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            WXNFile file = new WXNFile();

            WXNFileContent dingledong = file.Read(@"C:\Users\Pichau\source\repos\WINDTK\WINDTK\Sample2.wxn");
            Console.WriteLine(dingledong.ToString());
        }
    }
}
