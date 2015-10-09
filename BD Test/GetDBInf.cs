using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace BD_Test
{
    static class GetDBInf
    {
        public static void GetDbinf()
        {
            DataTable dt = new DataTable();

            MySqlConnectionStringBuilder mysqlCSB;

            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "fbd";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "1111";

            string queryString = @"INSERT INTO table1  VALUES (2)";

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mysqlCSB.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    com.CommandText = queryString;
                    //com.Parameters.Add("idtable1", MySqlDbType.);
                    //com.Parameters["idtable1"].Value = 1;
                    com.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
        }
        public static void GetData()
        {
            DataTable dt = new DataTable();


            MySqlConnectionStringBuilder mysqlCSB;

            mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "localhost";
            mysqlCSB.Database = "fbd";
            mysqlCSB.UserID = "root";
            mysqlCSB.Password = "1111";

            string queryString = @"SELECT idtable1 FROM table1";

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mysqlCSB.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    com.CommandText = queryString;
                    MySqlDataReader reader = com.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0}\t", reader.GetInt32(0));
                        }
                    }

                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }


        }
    }
}

