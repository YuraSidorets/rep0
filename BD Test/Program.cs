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
           

            worker.SetValue(@"F:\Flash\Protokol.docx", "new_table");

        }
    }
}
