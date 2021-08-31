using System;
using System.Collections.Generic;
using System.IO;

namespace WINDTK
{
    enum WXNTypes
    {
        Int, String, Bool, Array_Int, Array_String, Array_Bool
    }

    class WXNFile
    {
        private List<WXNPureObject> pureWriteMemory = new List<WXNPureObject>();
        private List<WXNObject> writeMemory = new List<WXNObject>();

        // Initializing
        bool IsPureObject(string text)
        {
            string formatedText = text.Trim();

            return formatedText[0] == '<' && formatedText[^1] == '>';
        }

        WXNObject DeconstructObject(string textInFile)
        {
            string[] FileInfoDivision = textInFile.Split(":");

            return new WXNObject(Enum.Parse<WXNTypes>(FileInfoDivision[0].Split("<")[1].Replace(">", "")), FileInfoDivision[0].Split("<")[0], FileInfoDivision[1]);
        }

        private WXNPureObject DeconstructPureObject(string textInFile)
        {
            string[] data = textInFile[1..^1].Trim().Split(':');
            dynamic parsedData;

            if (int.TryParse(data[1], out int dataIntParsed)) { parsedData = dataIntParsed; }
            else if (bool.TryParse(data[1], out bool dataBoolParsed)) { parsedData = dataBoolParsed; }
            else { parsedData = data[1].Replace('"', ' ').Trim(); }

            return new WXNPureObject(data[0], data[1].Trim());
        }

        // Utility functions

        // Reading
        public WXNFileContent Read(string FilePath)
        {
            var ReturnValue = new List<WXNObject>();
            var ReturnPureValue = new List<WXNPureObject>();

            // Reading file
            var FileAsText = new List<string>();
            try
            {
                string[] fileAsText = File.ReadAllText(FilePath).Split('\n', '\t');
                for (int i = 0; i < fileAsText.Length; i++)
                {
                    if (fileAsText[i] != "") { FileAsText.Add(fileAsText[i]); } 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new FileNotFoundException();
            }

            for (int i = 0; i < FileAsText.Count; i++)
            {
                // Reading normal objects
                if (!IsPureObject(FileAsText[i]))
                {
                    WXNObject @object = DeconstructObject(FileAsText[i]);

                    if (!@object.isArray)
                    {
                        switch (@object.type)
                        {
                            case WXNTypes.Int:
                                @object.data = int.Parse(@object.data);
                                break;
                            case WXNTypes.String:
                                @object.data = @object.data.Replace('"', ' ').Trim();
                                break;
                            case WXNTypes.Bool:
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
                            case WXNTypes.Array_Int:
                                int[] Value = new int[RawValueArray.Length];

                                for (int a = 0; a < RawValueArray.Length; a++) { Value[a] = int.Parse(RawValueArray[a]); }

                                // Applying value
                                @object.data = Value;

                                break;
                            case WXNTypes.Array_String:
                                for (int b = 0; b < RawValueArray.Length; b++) { RawValueArray[b] = RawValueArray[b].Replace('"', ' ').Trim(); }

                                // Applying value
                                @object.data = RawValueArray;

                                break;
                            case WXNTypes.Array_Bool:
                                bool[] value = new bool[RawValueArray.Length];

                                for(int c = 0; c < RawValueArray.Length; c++) { value[c] = bool.Parse(RawValueArray[c]); }

                                @object.data = value;

                                break;
                        }
                        ReturnValue.Add(@object);
                    }
                }
                else
                {
                    // Reading pure objects
                    WXNPureObject _object = DeconstructPureObject(FileAsText[i]);

                    ReturnPureValue.Add(_object);
                }
            }

            return new WXNFileContent(ReturnValue, ReturnPureValue);
        }

        // Writing
        public void ClearWriteMemory()
        {
            writeMemory.Clear();
            pureWriteMemory.Clear();
        }

        public void Write(WXNObject _object)
        {
            for (int i = 0; i < writeMemory.Count; i++)
            {
                if (_object.identifier == writeMemory[i].identifier)
                {
                    writeMemory[i] = _object;
                    return;
                }
            }
            writeMemory.Add(_object);
        }

        public void WritePure(WXNPureObject _object)
        {
            for (int i = 0; i < pureWriteMemory.Count; i++)
            {
                if (_object.identifier == pureWriteMemory[i].identifier)
                {
                    pureWriteMemory[i] = _object;
                    return;
                }
            }
            pureWriteMemory.Add(_object);
        }

        public void Save(string filePath)
        {
            string text = "";

            // Writing pure objects
            foreach (var item in pureWriteMemory)
            {
                text += $"<{item.identifier}:{item.data}>\n";
            }

            // Writing regular objects
            foreach (var item in writeMemory)
            {
                if (!item.isArray) { text += $"{item.identifier}<{item.type}>: {item.data}\n"; }
                else
                {
                    text += $"{item.identifier}<{item.type}>: [";

                    if (item.type == WXNTypes.Array_String)
                    {
                        for (int i = 0; i < item.data.Length - 1; i++) { text += $"\"{item.data[i]}\", "; }

                        text += $"\"{item.data[item.data.Length - 1]}\"]\n";
                    }
                    else
                    {
                        for (int i = 0; i < item.data.Length - 1; i++) { text += $"{item.data[i]}, "; }

                        text += $"{item.data[item.data.Length - 1]}]\n";
                    }
                }
            }

            // Saving
            File.WriteAllText(filePath, text);
        }
    }
}
