using System;
using System.Collections.Generic;
using System.IO;
using WINDTK.Types;

namespace WINDTK
{
    enum AcceptedTypes
    {
        Int, String, Bool, Array_Int, Array_String, Array_Bool
    }

    class WXNFile
    {
        static bool IsPureObject(string text)
        {
            string formatedText = text.Trim();
            return formatedText[0] == '<' && formatedText[^1] == '>';
        }

        static WXNObject DescontructObject(ref string textInFile)
        {
            string[] FileInfoDivision = textInFile.Split(":");
            return new WXNObject(Enum.Parse<AcceptedTypes>(FileInfoDivision[0].Split("<")[1].Replace(">", "")), FileInfoDivision[0].Split("<")[0], FileInfoDivision[1]);
        }

        // Utility functions
        public static WXNFileContent Read(string FilePath)
        {
            // Reading file
            string[] FileAsText;
            try
            { FileAsText = File.ReadAllLines(FilePath); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new FileNotFoundException();
            }
            var ReturnValue = new List<WXNObject>();
            var WXNData = new Dictionary<string, dynamic>();

            for (int i = 0; i < FileAsText.Length; i++)
            {
                if (!IsPureObject(FileAsText[i]))
                {
                    WXNObject @object = DescontructObject(ref FileAsText[i]);

                    if (!@object.isArray)
                    {
                        switch (@object.type)
                        {
                            case AcceptedTypes.Int:
                                @object.data = int.Parse(@object.data);
                                break;
                            case AcceptedTypes.String:
                                @object.data = @object.data.Replace('"', ' ').Trim();
                                break;
                            case AcceptedTypes.Bool:
                                @object.data = bool.Parse(@object.data);
                                break;
                        }
                        ReturnValue.Add(@object);
                    }
                    else
                    {
                        string[] RawValueArray = @object.data.Replace("[", "").Replace("]", "").Split(",");
                        switch (@object.type)
                        {
                            case AcceptedTypes.Array_Int:
                                int[] Value = new int[RawValueArray.Length];
                                for (int a = 0; a < RawValueArray.Length; a++)
                                    Value[a] = int.Parse(RawValueArray[a]);
                                // Applying value
                                @object.data = Value;
                                break;
                            case AcceptedTypes.Array_String:
                                for (int a = 0; a < RawValueArray.Length; a++)
                                {
                                    RawValueArray[a] = RawValueArray[a].Replace('"', ' ').Trim();
                                }
                                // Applying value
                                @object.data = RawValueArray;
                                break;
                            case AcceptedTypes.Array_Bool:
                                bool[] value = new bool[RawValueArray.Length];
                                for(int ii = 0; ii < RawValueArray.Length; ii++)
                                {
                                    value[ii] = bool.Parse(RawValueArray[ii]);
                                }
                                @object.data = value;
                                break;
                        }
                        ReturnValue.Add(@object);
                    }
                }
                else
                {
                    string[] data = FileAsText[i][1..^1].Split(':');

                    dynamic parsedData;
                    if (int.TryParse(data[1], out int dataIntParsed))
                        parsedData = dataIntParsed;
                    else if (bool.TryParse(data[1], out bool dataBoolParsed))
                        parsedData = dataBoolParsed;
                    else
                        parsedData = data[1].Replace('"', ' ').Trim();

                    if (!WXNData.ContainsKey(data[0]))
                        WXNData.Add(data[0], parsedData);
                    else
                        WXNData[data[0]] = parsedData;
                }
            }
            return new WXNFileContent(ReturnValue, WXNData);
        }

        public static void Write(string path, List<WXNObject> objects, Dictionary<string, dynamic> pureObjects)
        {
            string text = "";
            foreach (var item in pureObjects)
            {
                text += $"<{item.Key}:{item.Value}>\n";
            }
            foreach (var item in objects)
            {
                if (!item.isArray)
                {
                    text += $"{item.identifier}<{item.type}>: {item.data}\n";
                }
                else
                {
                    text += $"{item.identifier}<{item.type}>: [";
                    if (item.type == AcceptedTypes.Array_String)
                    {
                        for (int i = 0; i < item.data.Length - 1; i++)
                            text += $"\"{item.data[i]}\", ";
                        text += $"\"{item.data[item.data.Length - 1]}\"]\n";
                    }
                    else
                    {
                        for (int i = 0; i < item.data.Length - 1; i++)
                            text += $"{item.data[i]}, ";
                        text += $"{item.data[item.data.Length - 1]}]\n";
                    }
                }
            }
            File.WriteAllText(path, text);
        }
    }
}
