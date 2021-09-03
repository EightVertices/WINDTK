using System.Collections.Generic;

namespace WINDTK.WXN
{
    public struct WXNObject
    {
        public string identifier;
        public dynamic data;
        public WXNTypes type;
        public bool isArray;

        public WXNObject(WXNTypes type, string identifier, dynamic data)
        {
            this.data = data;
            this.type = type;
            this.identifier = identifier;
            isArray = type.ToString().Contains("Array");
        }
    }

    public struct WXNPureObject
    {
        public string identifier;
        public dynamic data;

        public WXNPureObject(string identifier, dynamic data)
        {
            this.identifier = identifier;
            this.data = data;
        }
    }

    public class WXNFileContent
    {
        public List<WXNObject> objects;
        public List<WXNPureObject> pureObjects;

        public WXNFileContent(List<WXNObject> objects, List<WXNPureObject> pureObjects)
        {
            this.objects = objects;
            this.pureObjects = pureObjects;
        }

        public override string ToString()
        {
            string returnValue = "";

            foreach (var item in pureObjects) { returnValue += $"id: {item.identifier} / Value: {item.data}\n"; }

            foreach (var item in objects) { returnValue += $"id: {item.identifier} / Type: {item.type} / Value: {item.data}\n"; }

            return returnValue;
        }

        public dynamic this[string ID, bool iteratePure = false]
        {
            get
            {
                if (!iteratePure)
                    return objects.Find(obj => obj.identifier == ID).data;
                else
                    return pureObjects.Find(obj => obj.identifier == ID).data;
            }
            set
            {
                if (!iteratePure)
                    objects.ForEach(obj => 
                    {
                        if (obj.identifier == ID)
                            obj.data = value;
                    });
                else
                    pureObjects.ForEach(obj =>
                    {
                        if (obj.identifier == ID)
                            obj.data = value;
                    });
            }
        }
    }
}
