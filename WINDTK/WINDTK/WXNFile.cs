using System;
using System.Collections.Generic;
using System.IO;

namespace WINDTK
{
    public enum WXNTypes
    {
        Int, String, Bool, Array_Int, Array_String, Array_Bool, Vector2, Vector3, Array_Vector2, Array_Vector3
    }

    public class WXNFile
    {
        private List<WXNPureObject> pureWriteMemory = new List<WXNPureObject>();
        private List<WXNObject> writeMemory = new List<WXNObject>();

        private bool IsPureObject(string text)
        {
            string formatedText = text.Trim();
            return formatedText[0] == '<' && formatedText[^1] == '>';
        }

        private dynamic GetPureObjectData(string text)
        {
            if (int.TryParse(text.Trim(), out int dataIntParsed)) 
                return dataIntParsed;
            else if (bool.TryParse(text.Trim(), out bool dataBoolParsed)) 
                return dataBoolParsed; 
            else 
                return text.Replace('"', ' ').Trim();
        }

        private WXNObject DeconstructObject(string textInFile)
        {
            string[] FileInfoDivision = textInFile.Split(":");
            return new WXNObject(Enum.Parse<WXNTypes>(FileInfoDivision[0].Split("<")[1].Replace(">", "")), FileInfoDivision[0].Split("<")[0], FileInfoDivision[1]);
        }

        // Reading
        public WXNFileContent Read(string FilePath)
        {
            if (Path.GetExtension(FilePath) != ".wxn")
                throw new Exception("The file is not a wxn file");

            var ReturnValue = new List<WXNObject>();
            var ReturnPureValue = new List<WXNPureObject>();

            // Reading file
            string[] FileAsText;
            try { FileAsText = File.ReadAllText(FilePath).Split(new[] { '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries); }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new FileNotFoundException();
            }

            for (int i = 0; i < FileAsText.Length; i++)
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
                                for (int a = 0; a < RawValueArray.Length; a++) 
                                    Value[a] = int.Parse(RawValueArray[a]);

                                // Applying value
                                @object.data = Value;
                                break;
                            case WXNTypes.Array_String:
                                for (int b = 0; b < RawValueArray.Length; b++) 
                                    RawValueArray[b] = RawValueArray[b].Replace('"', ' ').Trim();

                                // Applying value
                                @object.data = RawValueArray;
                                break;
                            case WXNTypes.Array_Bool:
                                bool[] value = new bool[RawValueArray.Length];
                                for(int c = 0; c < RawValueArray.Length; c++) 
                                    value[c] = bool.Parse(RawValueArray[c]);

                                @object.data = value;

                                break;
                        }
                        ReturnValue.Add(@object);
                    }
                }
                else
                {
                    WXNPureObject _object;

                    // Reading pure objects
                    string[] data = FileAsText[i][1..^1].Trim().Split(':');
                    if (data[1].Trim()[0] == '[' && data[1].Trim()[^1] == ']')
                    {
                        string[] newData = data[1].Replace("[", "").Replace("]", "").Split(",");
                        var dataArray = new dynamic[newData.Length];

                        for (int ii = 0; ii < newData.Length; ii++)
                            dataArray[ii] = GetPureObjectData(newData[ii]);

                        _object = new WXNPureObject(data[0], dataArray);
                    }
                    else
                    {
                        dynamic parsedData = GetPureObjectData(data[1]);
                        _object = new WXNPureObject(data[0], parsedData);
                    }

                    ReturnPureValue.Add(_object);
                }
            }

            return new WXNFileContent(ReturnValue, ReturnPureValue);
        }

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
                if (item.data.ToString().Contains("[]"))
                {
                    text += $"<{item.identifier}: [";
                    for (int i = 0; i < item.data.Length - 1; i++)
                        text += $"{item.data[i]}, ";

                    text += $"{item.data[item.data.Length - 1]}]>\n";
                }
                else
                {
                    text += $"<{item.identifier}:{item.data}>\n";
                }
            }

            // Writing regular objects
            foreach (var item in writeMemory)
            {
                if (!item.isArray) 
                    text += $"{item.identifier}<{item.type}>: {item.data}\n"; 
                else
                {
                    text += $"{item.identifier}<{item.type}>: [";

                    if (item.type == WXNTypes.Array_String)
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

            // Saving
            File.WriteAllText(filePath, text);
        }
    }
}
