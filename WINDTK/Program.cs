using System;

namespace WINDTK
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            WXNFile file = new WXNFile();
            WXNFileContent dingledong = file.Read(@"");
            Console.WriteLine(dingledong.ToString());
        }
    }
}
