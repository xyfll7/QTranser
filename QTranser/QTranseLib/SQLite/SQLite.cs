using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data;

namespace QTranser.QTranseLib
{
    class SQLite
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="dbPath">数据库文件夹（以用户名命名）(xyf)</param>
        /// <param name="dbName">数据库文件（以用户名命名）（xyf.sqlite)</param>
        private void Init(string dbUserName)
        { 
            string dbPath = Path.Combine(Environment.CurrentDirectory, dbUserName);
            string dbName = dbUserName + ".sqlite";
            dbName = dbName == null ? "database.sqlite" : dbName;

            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            if (!string.IsNullOrEmpty(dbName) && !File.Exists(Path.Combine(dbPath, dbName)))
                File.Create(Path.Combine(dbPath, dbName));

            config.DatabaseFile = dbName;
            config.DatabasePath = dbPath;
            config.DatabaseUser = dbUserName;
        }
        private void CreateTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();


                }
            }
        }
    }
}
