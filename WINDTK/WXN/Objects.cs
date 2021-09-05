namespace WINDTK.WXN
{
    public class WXNPureObject
    {
        public string identifier;
        public dynamic data;

        public WXNPureObject(string identifier, dynamic data)
        {
            this.identifier = identifier;
            this.data = data;
        }
    }

    public class WXNObject : WXNPureObject
    {
        public WXNTypes type;
        internal bool isArray;

        public WXNObject(WXNTypes type, string identifier, object data) : base(identifier, data)
        {
            this.data = data;
            this.type = type;
            this.identifier = identifier;
            isArray = type.ToString().Contains("Array");
        }
    }
}
