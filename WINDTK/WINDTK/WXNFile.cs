using System;
using System.Collections.Generic;
using System.IO;

namespace WINDTK
{
    enum AcceptedTypes
    {
        Int, String, Bool, Array_Int, Array_String, Array_Bool
    }

    struct WXNObject
    {
        public string identifier;
        public AcceptedTypes type;
        public bool isArray;
        public dynamic data;

        public WXNObject(AcceptedTypes type, string identifier, dynamic data)
        {
            this.data = data;
            this.type = type;
            this.identifier = identifier;
            isArray = type == AcceptedTypes.Array_Bool || type == AcceptedTypes.Array_String || type == AcceptedTypes.Array_Int ? true : false;
        }
    }

    class WXNFile
    {
        private bool IsPureObject(ref string text)
        {
            string formatedText = text.Replace(" ", "");
            return formatedText[0] == '<' && formatedText[^2] == '>';
        }

        private string[] GetDataInObject(string dataToGet)
        {
            return dataToGet[1..^2].Split(':');
        }

        private WXNObject DescontructObject(ref string textInFile)
        {
            string[] FileInfoDivision = textInFile.Split(":");
            return new WXNObject(Enum.Parse<AcceptedTypes>(FileInfoDivision[0].Split("<")[1].Replace(">", "")), FileInfoDivision[0].Split("<")[0], FileInfoDivision[1]);
        }

        // WXNFile properties
        string FilePath = "";

        public WXNFile(string filePath)
        {
            FilePath = filePath;
        }

        // Utility functions
        public List<WXNObject> Read()
        {
            // Reading file
            string[] FileAsText = File.ReadAllText(FilePath).Split("\n");
            var ReturnValue = new List<WXNObject>();
            var WXNData = new Dictionary<string, dynamic>();

            for (int i = 0; i < FileAsText.Length; i++)
            {
                if (!IsPureObject(ref FileAsText[i]))
                {
                    // File info
                    WXNObject @object = DescontructObject(ref FileAsText[i]);

                    // Reading value
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
                    var data = GetDataInObject(FileAsText[i]);
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

            foreach (var item in WXNData)
            {
                Console.WriteLine(item);
            }
            // Return
            return ReturnValue;
        }

        public void Write(string path, List<WXNObject> objects, Dictionary<string, dynamic> pureObjects)
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
                    text += $"{item.identifier}<{item.type}>: ";
                    text += "[";
                    for (int i = 0; i < item.data.Length - 1; i++)
                    {
                        text += $"{item.data[i]}, ";
                    }
                    text += $"{item.data[item.data.Length - 1]}]\n";
                }
            }
            File.WriteAllText(path, text);
        }
    }
}
