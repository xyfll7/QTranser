using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTestsWindow.SQlite
{
    class SQlite
    {
        private static string directory = Environment.CurrentDirectory;
        private static string fileName = @"xyf.sqlite";
        private static string path = Path.Combine(directory, fileName);
        static void CreateDB()
        {

            SQLiteConnection conn = new SQLiteConnection($"data source={path}");
            conn.Open();
            conn.Close();
        }

        static void DeleteDB()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        static void CreateTable()
        {
            SQLiteConnection conn = new SQLiteConnection($"data source={path}");
            if(conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = "CREATE TABLE t1(id varchar(4),score int)";
                //cmd.CommandText = "CREATE TABLE IF NOT EXISTS t1(id varchar(4),score int)";
                cmd.ExecuteNonQuery();


                // 更改表名
                cmd.CommandText = "ALTER TABLE t1 RENAME TO t3";
                cmd.ExecuteNonQuery();

                // 曾添列（字段）
                cmd.CommandText = "ALTER TABLE t1 ADD COLUMN age int";
                cmd.ExecuteNonQuery();

                // 插入数据
                cmd.CommandText = "INSERT INTO t1 VALUES('99999',11)";
                cmd.ExecuteNonQuery();

                // 变量插入
                string s = "123456";
                int n = 10;
                cmd.CommandText = "INSERT INTO t1(id,age) VALUES(@id,@age)";
                cmd.Parameters.Add("id", DbType.String).Value = s;
                cmd.Parameters.Add("age", DbType.Int32).Value = n;
                cmd.ExecuteNonQuery();


                // 替换数据
                string s0 = "123456";
                int n0 = 30;
                cmd.CommandText = "REPLACE INTO t1(id,age) VALUES(@id,@age)";
                cmd.Parameters.Add("id", DbType.String).Value = s0;
                cmd.Parameters.Add("age", DbType.Int32).Value = n0;
                cmd.ExecuteNonQuery();

                // 更新数据
                string s1 = "333444";
                int n1 = 30;
                cmd.CommandText = "UPDATE t1 SET id=@id,age=@age WHERE id='0123456789'";
                cmd.Parameters.Add("id", DbType.String).Value = s1;
                cmd.Parameters.Add("age", DbType.Int32).Value = n1;
                cmd.ExecuteNonQuery();


                // 删除数据
                cmd.CommandText = "DELETE FROM t1 WHERE id='99999'";
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }



        static void Query()
        {
            SQLiteConnection conn = new SQLiteConnection($"data source={path}");
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                //查询第1条记录，这个并不保险,rowid 并不是连续的，只是和当时插入有关
                cmd.CommandText = "SELECT * FROM t1 WHERE rowid=1";
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Console.WriteLine($"{sr.GetString(0)} {sr.GetInt32(1).ToString()}");
                }
                sr.Close();
                //运行以下的就能知道 rowid 并不能代表 行数
                cmd.CommandText = "SELECT rowid FROM t1 ";
                sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Console.WriteLine($"{sr.GetString(0)} {sr.GetInt32(1).ToString()}");
                }
                sr.Close();


                // 获取查询数据的行数（多少条记录）
                cmd.CommandText = "SELECT count(*) FROM t1";
                sr = cmd.ExecuteReader();
                sr.Read();
                Console.WriteLine(sr.GetInt32(0).ToString());
                sr.Close();

                // 整理数据库
                cmd.CommandText = "VACUUM";
                cmd.ExecuteNonQuery();
            }
        }

        //---事务
        //事务就是对数据库一组按逻辑顺序操作的执行单元。用事务的好处就是成熟的数据库都对 密集型的磁盘 IO 操作之类进行优化，而且还能进行撤回回滚操作。
        //其实在上面改变列名的示例中就用过。
        static void TransActionOperate(SQLiteConnection cn, SQLiteCommand cmd)
        {
            using (SQLiteTransaction tr = cn.BeginTransaction())
            {
                string s = "";
                int n = 0;
                cmd.CommandText = "INSERT INTO t2(id,score) VALUES(@id,@score)";
                cmd.Parameters.Add("id", DbType.String);
                cmd.Parameters.Add("score", DbType.Int32);
                for (int i = 0; i < 10; i++)
                {
                    s = i.ToString();
                    n = i;
                    cmd.Parameters[0].Value = s;
                    cmd.Parameters[1].Value = n;
                    cmd.ExecuteNonQuery();
                }
                tr.Commit();
            }
        }
        // 删除表
        // 和建立表的步骤一样，只是把 SQL 语句改了而已。
        static void DeleteTable()
        {
            SQLiteConnection conn = new SQLiteConnection($"data source={path}");
            if(conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DROP TABLE IF EXISTS t1";
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        // 查询表结构
        static void QueryTableStructure()
        {
            SQLiteConnection conn = new SQLiteConnection($"data source={path}");
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "PRAGMA table_info('t1')";

            //写法一：用DataAdapter和DataTable类，记得要 using System.Data
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            foreach (DataRow r in table.Rows)
            {
                Console.WriteLine($"{r["cid"]},{r["name"]},{r["type"]},{r["notnull"]},{r["dflt_value"]},{r["pk"]} ");
            }
            Console.WriteLine();

            //写法二：用DataReader，这个效率高些
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader[i]},");
                }
                Console.WriteLine();
            }
            reader.Close();
        }

        //---遍历查询表结构
        static void QueryAllTableInfo()
        {
            SQLiteConnection conn = new SQLiteConnection($"data source={path}");
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table' ";
                SQLiteDataReader sr = cmd.ExecuteReader();
                List<string> tables = new List<string>();
                while (sr.Read())
                {
                    tables.Add(sr.GetString(0));
                }
                //datareader 必须要先关闭，否则 commandText 不能赋值
                sr.Close();


                // 读取创建表
                cmd.CommandText = "SELECT sql FROM sqlite_master WHERE TYPE='table'";
                SQLiteDataReader sr0 = cmd.ExecuteReader();
                while (sr0.Read())
                {
                    Console.WriteLine(sr[0].ToString());
                }
                sr.Close();












                foreach (var a in tables)
                {
                    cmd.CommandText = $"PRAGMA TABLE_INFO({a})";
                    sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        Console.WriteLine($"{sr[0]} {sr[1]} {sr[2]} {sr[3]}");
                    }
                    sr.Close();
                }
            }
            conn.Close();
        }
        //方式一
        //---更改列名1
        //总思路：把旧表更名，建个带新列名的新表，拷贝数据
        //params string[] 中：0 数据库名，1 表名，2 旧列名 3 新列名
        static void RenameColumn1(params string[] str)
        {
            SQLiteConnection cn = new SQLiteConnection("data source=" + str[0]);
            cn.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = cn;

            //取得str[1]表名的表的建表SQL语句 
            cmd.CommandText = "SELECT name,sql FROM sqlite_master WHERE TYPE='table' ORDER BY name";
            SQLiteDataReader sr = cmd.ExecuteReader();

            string _sql = "";
            while (sr.Read())
            {
                if (string.Compare(sr.GetString(0), str[1], true) == 0)
                {
                    _sql = sr.GetString(1);
                    break;
                }
            }
            sr.Close();

            //更改旧表名为 带 _old 
            string _old = str[1] + "_old";
            cmd.CommandText = $"ALTER TABLE {str[1]} RENAME TO {_old}";
            cmd.ExecuteNonQuery();

            //建立新表，假设输入的旧列名和表中的列名大小写等完全一致，不写能容错的了
            _sql = _sql.Replace(str[2], str[3]);
            cmd.CommandText = _sql;
            cmd.ExecuteNonQuery();

            //拷贝数据
            using (SQLiteTransaction tr = cn.BeginTransaction())
            {
                cmd.CommandText = $"INSERT INTO {str[1]} SELECT * FROM {_old}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DROP TABLE {_old}";
                cmd.ExecuteNonQuery();
                tr.Commit();
            }
            cn.Close();
        }

        //方式二：

        //---更改列名2,改写schema里建表时的sql语句
        //原理：sqlite 每次打开的时候，都是依据建表时的sql语句来动态建立column的信息的
        static void RenameColumn2(params string[] str)
        {
            SQLiteConnection cn = new SQLiteConnection("data source=" + str[0]);
            cn.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = cn;

            //取得str[1]表名的表的建表SQL语句 
            cmd.CommandText = $"SELECT sql FROM sqlite_master WHERE TYPE='table' AND name='{str[1]}'";
            SQLiteDataReader sr = cmd.ExecuteReader();
            sr.Read();
            string _sql = sr.GetString(0);
            sr.Close();
            //注意单引号 '
            _sql = $"UPDATE sqlite_master SET sql='{_sql.Replace(str[2], str[3])}' WHERE name= '{str[1]}' ";

            //设置 writable_schema 为 true，准备改写schema 
            cmd.CommandText = "pragma writable_schema=1";
            cmd.ExecuteNonQuery();
            cmd.CommandText = _sql;
            cmd.ExecuteNonQuery();
            //设置 writable_schema 为 false。
            cmd.CommandText = "pragma writable_schema=0";
            cmd.ExecuteNonQuery();

            cn.Close();
        }


        #region 删除列

        //---删除列2，string[] ,0 数据库路径，1 表名，2 要删除的列名
        static void DeleteColumn2(params string[] str)
        {
            SQLiteConnection cn = new SQLiteConnection("data source=" + str[0]);
            cn.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = cn;
            //取得str[1]表名的表的建表SQL语句 
            cmd.CommandText = $"SELECT sql FROM sqlite_master WHERE TYPE='table' AND name='{str[1]}'";
            SQLiteDataReader sr = cmd.ExecuteReader();
            sr.Read();
            string _sql = sr.GetString(0);
            sr.Close();

            //取得列的定义
            //C#7.0的新特征，Tuple<>的语法糖，需要 NuGet install-package system.valuetuple
            List<(string name, string define)> list = GetColumnDefine(_sql);
            //取得要删除列的序号
            int _index = list.IndexOf(list.Where(x => x.name == str[2]).First());
            //建立新的sql语句
            StringBuilder sb = new StringBuilder();
            sb.Append($"CREATE TABLE {str[1]}(");
            for (int i = 0; i < list.Count; i++)
            {
                if (i != _index)
                {
                    sb.Append($"{list[i].define},");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");
            //改写schema
            _sql = $"UPDATE sqlite_master SET sql='{sb.ToString()}' WHERE name='{str[1]}'";
            //设置 writable_schema 为 true，准备改写schema 
            cmd.CommandText = "pragma writable_schema=1";
            cmd.ExecuteNonQuery();
            cmd.CommandText = _sql;
            cmd.ExecuteNonQuery();
            //设置 writable_schema 为 false。
            cmd.CommandText = "pragma writable_schema=0";
            cmd.ExecuteNonQuery();

            cn.Close();
        }
        //---取得列的定义
        static List<(string, string)> GetColumnDefine(string SqlStr)
        {
            int n = 0;
            int _start = 0;
            string _columnStr = "";
            for (int i = 0; i < SqlStr.Length; i++)
            {
                if (SqlStr[i] == '(')
                {
                    if (n++ == 0) { _start = i; }
                }
                else
                {
                    if (SqlStr[i] == ')')
                    {
                        if (--n == 0)
                        {
                            _columnStr = SqlStr.Substring(_start + 1, i - _start - 1);
                            break;
                        }
                    }

                }
            }
            string[] ss = _columnStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //C#7.0的新特征，Tuple<>的语法糖，需要 NuGet install-package system.valuetuple
            List<(string name, string define)> reslut = new List<(string name, string define)>();
            foreach (var a in ss)
            {
                string s = a.Trim();
                n = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        reslut.Add((s.Substring(0, i), s));
                        break;
                    }
                }
            }
            return reslut;
        }
        #endregion
    }
}
