using System;
using WINDTK;

namespace WINDTK
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var wxnFile = new WXNFile("testFile.wxn");
            var data = wxnFile.Read();
            foreach (var item in data)
            {
                Console.WriteLine(item.Value.ToString());
            }
        }
    }
}
