using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace WINDTK
{
    enum WXNSeparators
    {
        Vector = ';', Array = ',', ObjectValue = ':'
    }

    public enum WXNTypes
    {
        Int, Array_Int, String, Array_String, Bool, Array_Bool, Float, Array_Float, Vector, Array_Vector2, Array_Vector3
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
            if (text.Contains('(') && text.Contains(')'))
            {
                string[] rawVector = text.Replace("<", "").Replace(">", "").Replace('.', ',').Split((char)WXNSeparators.Vector);
                if (rawVector.Length < 3)
                    return new Vector2(float.Parse(rawVector[0]), float.Parse(rawVector[1]));
                else
                    return new Vector3(float.Parse(rawVector[0]), float.Parse(rawVector[1]), float.Parse(rawVector[2]));
            }
            else
            {
                if (int.TryParse(text.Trim(), out int dataIntParsed)) 
                    return dataIntParsed;
                else if (bool.TryParse(text.Trim(), out bool dataBoolParsed)) 
                    return dataBoolParsed; 
                else if (float.TryParse(text.Replace('.', ','), out float dataFloatParsed))
                    return dataFloatParsed;
                else
                    return text.Replace('"', ' ').Trim();
            }
        }

        private WXNObject DeconstructObject(string textInFile)
        {
            string[] FileInfoDivision = textInFile.Split((char)WXNSeparators.ObjectValue);
            return new WXNObject(Enum.Parse<WXNTypes>(FileInfoDivision[0].Split("<")[1].Replace(">", "")), FileInfoDivision[0].Split("<")[0], FileInfoDivision[1]);
        }

        private bool CheckExistentID(List<WXNObject> WXNObjects, WXNObject _object)
        {
            for (int i = 0; i < WXNObjects.Count; i++)
            {
                if (_object.identifier == WXNObjects[i].identifier)
                {
                    WXNObjects[i] = _object;
                    return true;
                }
            }
            return false;
        }

        private bool CheckExistentPureID(List<WXNPureObject> returnPureValue, WXNPureObject _object)
        {
            for (int i = 0; i < returnPureValue.Count; i++)
            {
                if (_object.identifier == returnPureValue[i].identifier)
                {
                    returnPureValue[i] = _object;
                    return true;
                }
            }
            return false;
        }

        private dynamic DeconstructVector(string rawVector)
        {
            string[] vectorValues = rawVector.Replace("<", "").Replace(">", "").Replace('.', ',').Split((char)WXNSeparators.Vector);
            if (vectorValues.Length > 2)
                return new Vector3(float.Parse(vectorValues[0]), float.Parse(vectorValues[1]), float.Parse(vectorValues[2]));
            else
                return new Vector2(float.Parse(vectorValues[0]), float.Parse(vectorValues[1]));
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
                            case WXNTypes.Float:
                                @object.data = float.Parse(@object.data.Replace('.', ','));
                                break;
                            case WXNTypes.Vector:
                                @object.data = DeconstructVector(@object.data);
                                break;
                        }
                    }
                    else
                    {
                        string[] RawValueArray = @object.data.Replace("[", "").Replace("]", "").Split((char)WXNSeparators.Array);

                        switch (@object.type)
                        {
                            case WXNTypes.Array_Int:
                                int[] rawIntValue = new int[RawValueArray.Length];
                                for (int a = 0; a < RawValueArray.Length; a++)
                                    rawIntValue[a] = int.Parse(RawValueArray[a]);

                                @object.data = rawIntValue;
                                break;
                            case WXNTypes.Array_String:
                                for (int b = 0; b < RawValueArray.Length; b++)
                                    RawValueArray[b] = RawValueArray[b].Replace('"', ' ').Trim();

                                @object.data = RawValueArray;
                                break;
                            case WXNTypes.Array_Bool:
                                bool[] rawBooleanValue = new bool[RawValueArray.Length];
                                for (int c = 0; c < RawValueArray.Length; c++)
                                    rawBooleanValue[c] = bool.Parse(RawValueArray[c]);

                                @object.data = rawBooleanValue;
                                break;
                            case WXNTypes.Array_Float:
                                float[] rawFloatValue = new float[RawValueArray.Length];
                                for (int c = 0; c < RawValueArray.Length; c++)
                                    rawFloatValue[c] = float.Parse(RawValueArray[c].Replace('.', ','));

                                @object.data = rawFloatValue;
                                break;
                            case WXNTypes.Array_Vector2:
                                Vector2[] rawVector2Value = new Vector2[RawValueArray.Length];
                                for (int c = 0; c < RawValueArray.Length; c++)
                                    rawVector2Value[c] = DeconstructVector(RawValueArray[c]);

                                @object.data = rawVector2Value;
                                break;
                            case WXNTypes.Array_Vector3:
                                Vector3[] rawVector3Value = new Vector3[RawValueArray.Length];
                                for (int c = 0; c < RawValueArray.Length; c++)
                                    rawVector3Value[c] = DeconstructVector(RawValueArray[c]);

                                @object.data = rawVector3Value;
                                break;
                        }
                    }

                    if (!CheckExistentID(ReturnValue, @object))
                        ReturnValue.Add(@object);
                }
                else
                {
                    WXNPureObject _object;
                    
                    string[] data = FileAsText[i][1..^1].Trim().Split((char)WXNSeparators.ObjectValue);
                    if (data[1].Trim()[0] == '[' && data[1].Trim()[^1] == ']')
                    {
                        string[] newData = data[1].Replace("[", "").Replace("]", "").Split((char)WXNSeparators.Array);
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

                    if (!CheckExistentPureID(ReturnPureValue, _object))
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

        public void Write(params WXNObject[] _object)
        {
            for (int i = 0; i < _object.Length; i++)
            {
                if (CheckExistentID(writeMemory, _object[i]))
                    return;
                writeMemory.Add(_object[i]);
            }
        }

        public void WritePure(params WXNPureObject[] _object)
        {
            for (int i = 0; i < _object.Length; i++)
            {
                if (CheckExistentPureID(pureWriteMemory, _object[i]))
                    return;
                pureWriteMemory.Add(_object[i]);
            }
        }

        public void Save(string filePath)
        {
            string text = "";

            // Writing pure objects
            foreach (var item in pureWriteMemory)
            {
                if (item.data.ToString().Contains("[]"))
                {
                    if (item.data.ToString().Contains('<') && item.data.ToString().Contains('>'))
                    {
                        text += $"<{item.identifier}: [";
                        for (int i = 0; i < item.data.Length - 1; i++)
                            text += $"{item.data[i].ToString()}; ";

                        text += $"{item.data[item.data.Length - 1].ToString()}]>\n";
                    }
                    else
                    {
                        text += $"<{item.identifier}: [";
                        for (int i = 0; i < item.data.Length - 1; i++)
                            text += $"{item.data[i]}, ";

                        text += $"{item.data[item.data.Length - 1]}]>\n";
                    }
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
                {
                    text += $"{item.identifier}<{item.type}>: {item.data.ToString().Replace('.', ';').Replace(',', '.')}\n";
                }
                else
                {
                    text += $"{item.identifier}<{item.type}>: [ ";
                    switch (item.type)
                    {
                        case WXNTypes.Array_String:
                            for (int i = 0; i < item.data.Length - 1; i++)
                                text += $"\"{item.data[i]}\", ";

                            text += $"\"{item.data[item.data.Length - 1]}\"]\n";
                            break;
                        case WXNTypes.Array_Vector2:
                        case WXNTypes.Array_Vector3:
                            for (int i = 0; i < item.data.Length - 1; i++)
                                text += $"{item.data[i].ToString().Replace('.', ';').Replace(',', '.')}; ";

                            text += $"{item.data[item.data.Length - 1].ToString().Replace('.', ';').Replace(',', '.')}]\n";
                            break;
                        default:
                            for (int i = 0; i < item.data.Length - 1; i++)
                                text += $"{item.data[i]}, ";

                            text += $"{item.data[item.data.Length - 1]}]\n";
                            break;
                    }
                }
            }

            // Saving
            File.WriteAllText(filePath, text);
        }
    }
}
