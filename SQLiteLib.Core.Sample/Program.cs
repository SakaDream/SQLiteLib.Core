using System;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Reflection;

namespace SQLiteLib.Core.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string dbName = "";
            bool saveJson = false;
            bool isEncrypt = false;

            Console.WriteLine("Welcome to SQLiteLib.Core Demo!");
            Console.WriteLine("Created by SakaDream");
            Console.WriteLine("---------------------------------------------");

            Console.WriteLine("Before creating database. Please setting some things...");
            Console.Write("Your database name: ");
            dbName = Console.ReadLine();
            while (true)
            {
                Console.Write("Save connection string to json file? (y / n)? ");
                string ans = Console.ReadLine();
                if (ans.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                {
                    saveJson = true;
                    while (true)
                    {
                        Console.Write("Encrypt json file? (y / n)? ");
                        string ansEncrypt = Console.ReadLine();
                        if (ansEncrypt.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                        {
                            isEncrypt = true;
                            break;
                        }
                        else if (ansEncrypt.Equals("n", StringComparison.CurrentCultureIgnoreCase))
                        {
                            isEncrypt = false;
                            break;
                        }
                    }
                    break;
                }
                else if (ans.Equals("n", StringComparison.CurrentCultureIgnoreCase))
                {
                    saveJson = false;
                    break;
                }
            }

            Console.WriteLine("Creating {0}...", dbName);
            try
            {
                File.Delete(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\" + dbName);
            }
            catch{}
            SQLiteConfig config = new SQLiteConfig();
            config.dataSource = dbName;
            if(saveJson)
            {
                SQLiteConfigJson.save(config, isEncrypt);
            }
            SQLiteLib.updateQuery(@"CREATE TABLE 'TEST' ('ID' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 'NAME' TEXT NOT NULL, 'GENDER' INTEGER NOT NULL DEFAULT 0 CHECK(GENDER == 0 OR GENDER == 1), 'SALARY' REAL DEFAULT 0)");

            Console.WriteLine("---------------------------------------------");
            string name;
            int gender, salary;
            Console.WriteLine("Please input some information");
            Console.Write("Name: ");
            name = Console.ReadLine();
            Console.Write("Gender (1 is male, 0 is female) : ");
            gender = Convert.ToInt32(Console.ReadLine());
            Console.Write("Salary: ");
            salary = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Inserting record to database...");
            SQLiteLib.updateQuery(string.Format(@"INSERT INTO TEST (NAME, GENDER, SALARY) VALUES ('{0}', {1}, {2})", name, gender, salary));

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Show all records in database...");
            SqliteDataReader reader = SQLiteLib.selectQuery(@"SELECT * FROM TEST");
            while (reader.Read())
            {
                Console.WriteLine("ID: " + reader[0]);
                Console.WriteLine("Name: " + reader[1]);
                Console.WriteLine("Gender: " + reader[2]);
                Console.WriteLine("Salary: " + reader[3]);
            }

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
