using System;
using System.Collections.Generic;
using System.IO;

namespace WINDTK
{
    class WXNFile
    {
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
            var ReturnValue = new Dictionary<string, dynamic>();

            // Reading file
            string[] FileAsText = File.ReadAllText(FilePath).Split("\n");

            string[] WXNData;

            for (int i = 0; i < FileAsText.Length; i++)
            {
                if (i > 0)
                {
                    // File info
                    string ValueName = "";
                    string ValueType = "";
                    string RawValue = "";
                    bool IsArray = false;

                    // Getting file info
                    string[] FileInfoDivision = FileAsText[i].Split(":");

                    ValueName = FileInfoDivision[0].Split("<")[0];
                    ValueType = FileInfoDivision[0].Split("<")[1].Replace(">", "");

                    RawValue = FileInfoDivision[1];

                    if (FileInfoDivision[1].Split("[").Length > 1) { IsArray = true; }

                    // Reading value
                    if (IsArray == false)
                    {
                        if (ValueType == "String")
                        {
                            string Value = RawValue.Replace('"', ' ').Replace(" ", "");

                            // Applying value
                            ReturnValue.Add(ValueName, Value);
                        }

                        if (ValueType == "Int")
                        {
                            int Value = int.Parse(RawValue);

                            // Applying value
                            ReturnValue.Add(ValueName, Value);
                        }
                    }
                    else
                    {
                        if (ValueType == "String")
                        {
                            string[] RawValueArray = RawValue.Replace("[", "").Replace("]", "").Split(",");

                            for (int a = 0; a < RawValueArray.Length; a++)
                            {
                                RawValueArray[a] = RawValueArray[a].Replace('"', ' ').Replace(" ", "");
                            }

                            // Applying value
                            ReturnValue.Add(ValueName, RawValueArray);
                        }

                        if (ValueType == "Int")
                        {
                            string[] RawValueArray = RawValue.Replace("[", "").Replace("]", "").Split(",");
                            int[] Value = new int[RawValueArray.Length];

                            for (int a = 0; a < RawValueArray.Length; a++)
                            {
                                Value[a] = int.Parse(RawValueArray[a]);
                            }

                            // Applying value
                            ReturnValue.Add(ValueName, Value);
                        }
                    }
                }
                else
                {
                    WXNData = FileAsText[0].Replace("<", "").Replace(">", "").Split(",");
                }
            }

            // Return
            return ReturnValue;
        }
    }
}
