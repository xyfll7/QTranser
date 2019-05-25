using System.Windows;

namespace QTranser.QTranseLib
{
    internal static class Idkey
    {
        private static string _id = "";
        private static string _key = "";
        public static string Id
        {
            get
            {
                if (_id == "")
                {
                    idkdy();
                }
             
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public static string Key
        {
            get
            {
                if (_key == "") idkdy();
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        public async static void idkdy()
        {
            await Getidkey.getidkey();
        }
    }
}
