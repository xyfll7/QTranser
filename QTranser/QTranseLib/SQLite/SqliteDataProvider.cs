using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTranser.QTranseLib.SQLite
{
    public class SqliteDataProvider : IDataProvider
    {
        #region Constants

        public const string FileName = "historyWords.db";

        #endregion Constants

        #region Constructors

        public SqliteDataProvider()
        {
            DataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = DataPath;
            ConnectionString = builder.ToString();

            if (!File.Exists(DataPath))
            {
                SQLiteConnection.CreateFile(DataPath);

                string sqlCreateTable = @"CREATE TABLE HistoryWord(
Id int Primary Key  Not Null,
Times          int  Not Null,
Word           text Not Null,
Translate      text)";

                ExeNonQueryCommand(sqlCreateTable);
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The full data path of the sqlite database file
        /// </summary>
        public string DataPath { get; private set; }

        /// <summary>
        /// The connection string
        /// </summary>
        protected string ConnectionString { get; set; }

        #endregion Properties

        #region Methods

        public bool Delete(IHistoryWord historyWord)
        {
            string sqlDelete = $@"DELETE FROM HistoryWord WHERE Id='{historyWord.Id}'";
            return ExeNonQueryCommand(sqlDelete);
        }

        public List<IHistoryWord> GetAllFriends()
        {
            var list = new List<IHistoryWord>();
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sqlInsert = $@"SELECT * FROM HistoryWord";
                using (SQLiteCommand cmd = new SQLiteCommand(sqlInsert, conn))
                {
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                    {
                        var dataTable = new DataTable();
                        da.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            var historyWord = new HistoryWord();
                            historyWord.Id = row["Id"].ToString();
                            historyWord.Times = (uint)row["Times"];
                            historyWord.Word = row["Word"] != null ? row["Word"].ToString() : string.Empty;
                            historyWord.Translate = row["Translate"] != null ? row["Translate"].ToString() : string.Empty;
                            list.Add(historyWord);
                        }
                    }
                }
            }

            return list;
        }

        public IHistoryWord GetFriendById(string id)
        {
            HistoryWord historyWord = null;
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sqlInsert = $@"SELECT * FROM HistoryWord WHERE Id='{id}'";
                using (SQLiteCommand cmd = new SQLiteCommand(sqlInsert, conn))
                {
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        historyWord = new HistoryWord();
                        historyWord.Id = dr["Id"].ToString();
                        historyWord.Times = (uint)dr["Name"];
                        historyWord.Word = dr["Email"] != null ? dr["Email"].ToString() : string.Empty;
                        historyWord.Translate = dr["Translate"] != null ? dr["Translate"].ToString() : string.Empty;
                    }
                }
            }

            return historyWord;
        }

        public bool Insert(IHistoryWord historyWord)
        {
            string sqlInsert = $@"INSERT INTO HistoryWord VALUES('{historyWord.Id}','{historyWord.Times}','{historyWord.Word}','{historyWord.Translate}')";
            return ExeNonQueryCommand(sqlInsert);
        }

        public bool Update(IHistoryWord historyWord)
        {
            string sqlUpdate = $@"UPDATE HistoryWord SET Word='{historyWord.Word}', Translate='{historyWord.Translate}', Times='{historyWord.Times}', WHERE Id='{historyWord.Id}'";
            return ExeNonQueryCommand(sqlUpdate);
        }

        private bool ExeNonQueryCommand(string sqlCommandText)
        {
            bool isSuccess = false;

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(sqlCommandText, conn))
                {
                    isSuccess = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }

            return isSuccess;
        }

        #endregion Methods
    }
}
