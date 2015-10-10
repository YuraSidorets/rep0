using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DB_Test
{
    class Program
    {
        static void Main(string[] args)
        {
           
            MySqlConnectionStringBuilder mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "mysql.sidoretsyura.myjino.ru";
            mysqlCSB.Database = "sidoretsyura_testdb";
            mysqlCSB.UserID = "sidoretsyura";
            mysqlCSB.Password = "123456";

            DBWorker worker = new DBWorker(mysqlCSB);
            Dictionary<string, string> a = FileWorker.GetFileInfo(@"F:\111\avstralia.jpg");

            worker.SetValue(@"F:\Backup\Music\Turbowolf\Turbowolf_-_Two_Hands_2015\01 - Invisible Hand.mp3", "new_table");

        }
    }
}
