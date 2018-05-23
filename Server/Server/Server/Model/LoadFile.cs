using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Data.SQLite;

namespace Server
{
    public class LoadFile
    {
        string _path = System.Windows.Forms.Application.StartupPath + "\\database.db";
        private SQLiteConnection sqlite_connect;
        private SQLiteCommand sqlite_cmd;
        public LoadFile() {
            if (!File.Exists(_path))
            {
                SQLiteConnection.CreateFile("database.db");
                CreatIndex();
            }
        }

        public void CreatIndex (){
            sqlite_connect = new SQLiteConnection("data source = database.db");
            sqlite_connect.Open();
            sqlite_cmd = sqlite_connect.CreateCommand();
            sqlite_cmd.CommandText = @"
CREATE TABLE IndexList (
	Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	Name VARCHAR ( 32 )
);";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_connect.Close();
        }

        public List<string> ReadData(string table,string condition)
        {
            sqlite_connect = new SQLiteConnection("data source = database.db");
            sqlite_connect.Open();
            sqlite_cmd = sqlite_connect.CreateCommand();
            sqlite_cmd.CommandText = @"select * from " + table+ " where " + condition + ");";
            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<string> data = new List<string>();
            while (sqlite_datareader.Read()) //read every data
            {
                string listData = sqlite_datareader["Type"].ToString() + ":" + sqlite_datareader["X"].ToString() + " " + sqlite_datareader["Y"].ToString() + " " + sqlite_datareader["Z"].ToString();
                data.Add(listData);
            }
            sqlite_connect.Close();
            return data;
        }

        public List<string> ReadData(string table)
        {
            sqlite_connect = new SQLiteConnection("data source = database.db");
            sqlite_connect.Open();
            sqlite_cmd = sqlite_connect.CreateCommand();
            sqlite_cmd.CommandText = @"select * from " + table + ";";
            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();

            List<string> data = new List<string>();
            while (sqlite_datareader.Read()) //read every data
            {
                string listData = sqlite_datareader["Type"].ToString() + ":" + sqlite_datareader["X"].ToString() + " " + sqlite_datareader["Y"].ToString() + " " + sqlite_datareader["Z"].ToString();
                data.Add(listData);
            }
            sqlite_connect.Close();
            return data;
        }

        public void InsetData(string table,string column, string value)
        {
            sqlite_connect = new SQLiteConnection("data source = database.db");
            sqlite_connect.Open();
            sqlite_cmd = sqlite_connect.CreateCommand();
            sqlite_cmd.CommandText = @"INSERT INTO " + table + " (" + column + ") values (" + value + ");";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_connect.Close();
        }

        public void CreatFile(string tableName)
        {
            sqlite_connect = new SQLiteConnection("data source = database.db");
            sqlite_connect.Open();
            sqlite_cmd = sqlite_connect.CreateCommand();
            sqlite_cmd.CommandText = @"
CREATE TABLE " + tableName + @" (
    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    Type VARCHAR(32) ,
	X integer DEFAULT 0,
	Y integer DEFAULT 0,
	Z integer DEFAULT 0
);";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = @"INSERT INTO IndexList (Name) values ('" + tableName + "');";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_connect.Close();
        }
    }
}
