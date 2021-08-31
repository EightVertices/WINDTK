using System;
using System.Collections.Generic;
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
                Console.WriteLine(item.data);
            }
            wxnFile.Write(@"testWrite.wxn", 
                new List<WXNObject>() { new WXNObject(AcceptedTypes.Array_Int, "Ages", new int[] { 14, 38, 39 }), new WXNObject(AcceptedTypes.Bool, "IsMachoMan", true) },
                new Dictionary<string, dynamic> { { "Version", 1 } });
        }
    }
}
