using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data; //test


namespace DB_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //DBWorker worker = new DBWorker();
            ////worker.SetValue(@"E:\1.txt", "new_table");
            //DataTable dt = worker.ReadValues();
            //foreach (DataRow row in dt.Rows)
            //{
            //    foreach (var value in row.ItemArray)
            //    {
            //        Console.WriteLine(value);
            //    }
            //    Console.WriteLine(new string('-', 30));
            //}
            FileWorker.GetFileFromDB(15, @"E:\");
            Console.ReadKey();
        }
    }
}
