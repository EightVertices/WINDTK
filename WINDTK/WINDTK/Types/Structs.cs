using System.Collections.Generic;

namespace WINDTK.Types
{
    struct WXNObject
    {
        public string identifier;
        public WXNTypes type;
        public bool isArray;
        public dynamic data;

        public WXNObject(WXNTypes type, string identifier, dynamic data)
        {
            this.data = data;
            this.type = type;
            this.identifier = identifier;
            isArray = type == WXNTypes.Array_Bool || type == WXNTypes.Array_String || type == WXNTypes.Array_Int ? true : false;
        }
    }

    struct WXNPureObject
    {
        public string identifier;
        public dynamic data;

        public WXNPureObject(string identifier, dynamic data)
        {
            this.identifier = identifier;
            this.data = data;
        }
    }

    class WXNFileContent
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

            foreach (var item in pureObjects)
            {
                returnValue += "Id: " + item.identifier + " / $: " + item.data;
                returnValue += "\n";
            }

            foreach (var item in objects)
            {
                returnValue += "Id: " + item.identifier + " / $: " + item.data;
                returnValue += "\n";
            }

            return returnValue;
        }
    }
}
