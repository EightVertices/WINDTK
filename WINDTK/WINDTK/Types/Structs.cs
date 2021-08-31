using System.Collections.Generic;

namespace WINDTK.Types
{
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

    struct WXNFileContent
    {
        public List<WXNObject> objects;
        public Dictionary<string, dynamic> pureObjects;

        public WXNFileContent(List<WXNObject> objects, Dictionary<string, dynamic> pureObjects)
        {
            this.objects = objects;
            this.pureObjects = pureObjects;
        }
    }
}
