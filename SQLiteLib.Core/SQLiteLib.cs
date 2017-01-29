using System;
using Microsoft.Data.Sqlite;

namespace SQLiteLib.Core
{
    public static class SQLiteLib
    {
        static SqliteConnection con;
        static SqliteCommand com;
        // public static void createDB(string path, bool saveConfig, bool encrypt)
        // {
        //     SqliteConnection.CreateFile(path);
        //     if (saveConfig)
        //     {
        //         SQLiteConfig config = new SQLiteConfig();
        //         config.dataSource = path;
        //         SQLiteConfigJson.save(config, encrypt);
        //     }
        //     else
        //     {
        //         SQLiteConfig config = new SQLiteConfig();
        //         config.dataSource = path;
        //         SQLiteConfigJson.saveTemp(config, encrypt);
        //     }
        // }
        // public static void createDB(string path, string password, bool saveConfig, bool encrypt)
        // {
        //     SqliteConnection.CreateFile(path);
        //     if (!password.Equals(""))
        //     {
        //         SqliteConnection con = new SqliteConnection("Data Source: " + path + ";");
        //         con.SetPassword(password);
        //     }
        //     if (saveConfig)
        //     {
        //         SQLiteConfig config = new SQLiteConfig();
        //         config.dataSource = path;
        //         if (!password.Equals(""))
        //         {
        //             config.password = password;
        //         }
        //         SQLiteConfigJson.save(config, encrypt);
        //     }
        //     else
        //     {
        //         SQLiteConfig config = new SQLiteConfig();
        //         config.dataSource = path;
        //         if (!password.Equals(""))
        //         {
        //             config.password = password;
        //         }
        //         SQLiteConfigJson.saveTemp(config, encrypt);
        //     }
        // }
        public static void connect(SQLiteConfig config)
        {
            con = new SqliteConnection(
                "DataSource = " + config.dataSource + ";"
                + "Mode = " + config.mode + ";"
                + "Cache = " + config.cache
                );

            con.Open();
            com = con.CreateCommand();
        }
        public static void connect()
        {
            try
            {
                SQLiteConfig config = SQLiteConfigJson.openTemp();
                connect(config);
            }
            catch
            {
                SQLiteConfig config = SQLiteConfigJson.open();
                connect(config);
            }
        }
        public static SqliteDataReader selectQuery(string query)
        {
            try
            {
                connect();
                com.CommandText = query;
                return com.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                if (con != null)
                {
                    con.Close();
                }
                return null;
            }
        }
        public static void updateQuery(string query)
        {
            try
            {
                connect();
                com.CommandText = query;
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                if (con != null)
                {
                    con.Close();
                }
            }
        }
    }
}