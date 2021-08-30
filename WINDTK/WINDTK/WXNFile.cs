using System;
using System.Collections.Generic;
using System.IO;

namespace WINDTK
{
    enum AcceptedTypes
    {
        Int, String, Bool
    }

    struct WXNObject
    {
        public string type, identifier;
        public bool isArray;
        public dynamic data;

        public WXNObject(string type, string identifier, dynamic data)
        {
            this.data = data;
            this.type = type;
            this.identifier = identifier;
            isArray = data.Split("[").Length > 1 ? true : false;
        }
    }

    class WXNFile
    {
        private string[] GetDataInObject(string dataToGet)
        {
            return dataToGet.Split('<')[1].Split('>')[0].Trim().Split(':');
        }

        private WXNObject DescontructObject(ref string textInFile)
        {
            string[] FileInfoDivision = textInFile.Split(":");
            return new WXNObject(FileInfoDivision[0].Split("<")[1].Replace(">", ""), FileInfoDivision[0].Split("<")[0], FileInfoDivision[1]);
        }

        // WXNFile properties
        string FilePath = "";

        public WXNFile(string filePath)
        {
            FilePath = filePath;

            Initialize();
        }

        private void Initialize()
        {

        }

        // Utility functions
        public Dictionary<string, dynamic> Read()
        {
            // Reading file
            string[] FileAsText = File.ReadAllText(FilePath).Split("\n");
            var ReturnValue = new Dictionary<string, dynamic>();
            var WXNData = new Dictionary<string, dynamic>();

            for (int i = 0; i < FileAsText.Length; i++)
            {
                if (FileAsText[i][0] != '<')
                {
                    // File info
                    WXNObject @object = DescontructObject(ref FileAsText[i]);

                    // Reading value
                    if (!@object.isArray)
                    {
                        switch (Enum.Parse<AcceptedTypes>(@object.type))
                        {
                            case AcceptedTypes.Int:
                                // Applying value
                                ReturnValue.Add(@object.identifier, int.Parse(@object.data));
                                break;
                            case AcceptedTypes.String:
                                // Applying value
                                ReturnValue.Add(@object.identifier, @object.data.Replace('"', ' ').Replace(" ", ""));
                                break;
                            case AcceptedTypes.Bool:
                                ReturnValue.Add(@object.identifier, bool.Parse(@object.data));
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        string[] RawValueArray = @object.data.Replace("[", "").Replace("]", "").Split(",");
                        switch (Enum.Parse<AcceptedTypes>(@object.type))
                        {
                            case AcceptedTypes.Int:
                                int[] Value = new int[RawValueArray.Length];
                                for (int a = 0; a < RawValueArray.Length; a++)
                                {
                                    Value[a] = int.Parse(RawValueArray[a]);
                                }
                                // Applying value
                                ReturnValue.Add(@object.identifier, Value);
                                break;
                            case AcceptedTypes.String:
                                for (int a = 0; a < RawValueArray.Length; a++)
                                {
                                    RawValueArray[a] = RawValueArray[a].Replace('"', ' ').Trim();
                                }
                                // Applying value
                                ReturnValue.Add(@object.identifier, RawValueArray);
                                break;
                            case AcceptedTypes.Bool:
                                break;
                        }
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

            // Return
            return ReturnValue;
        }
    }
}
