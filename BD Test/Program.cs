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

            worker.SetValue("new_table",a.Keys.ToArray()[0],a.Values.ToArray()[0
                ]);
            
        }
    }
}
