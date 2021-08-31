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
            WXNFile.Write(@"C:\Users\Pichau\source\repos\WINDTK\WINDTK\Write.wxn", 
                new List<WXNObject>() { 
                    new WXNObject(AcceptedTypes.String, "Professor DingledongGolden Luxury Statue", "LEGAL"),
                    new WXNObject(AcceptedTypes.Array_Bool, "CRIANÇAS QUE SOFREM BULLING", new bool[] { true, false, true, false }),
                    new WXNObject(AcceptedTypes.Int, "IntNum", 1),
                },
            new Dictionary<string, dynamic> { { "Version", 1 } });

            var wxnFile = WXNFile.Read(@"C:\Users\Pichau\source\repos\WINDTK\WINDTK\testFile.wxn");
            var wxnFile2 = WXNFile.Read(@"C:\Users\Pichau\source\repos\WINDTK\WINDTK\Write.wxn");

            foreach (var item in wxnFile.objects)
            {
                Console.WriteLine($"{item.identifier} : {item.data}");
            }
            Console.Write("\n");
            foreach (var item in wxnFile.pureObjects)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
            Console.Write("\n\n");

            foreach (var item in wxnFile2.objects)
            {
                Console.WriteLine($"{item.identifier} : {item.data}");
            }
            Console.Write("\n");
            foreach (var item in wxnFile2.pureObjects)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

        }
    }
}
