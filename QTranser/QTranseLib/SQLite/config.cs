using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTranser.QTranseLib
{
    class config
    {
        public static string DatabaseFile = "";
        public static string DatabasePath = "";
        public static string DatabaseUser = "";
        public static string DataSource
        {
            get
            {
                return string.Format("data source={0}{1};Version=3;", DatabasePath, DatabaseFile);
            }
        }
    }
}
