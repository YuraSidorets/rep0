using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

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

        public static Dictionary<string, string> GetFileInfo(string path)
        {
            Dictionary<string, string> fileInformation = new Dictionary<string, string>();
            string fileName = Path.GetFileNameWithoutExtension(path);
            fileInformation.Add("FileName", fileName);

            string fileType = Path.GetExtension(path);
            fileInformation.Add("FileType", fileType);

            DateTime fileCreationDate = File.GetCreationTime(path);
            fileInformation.Add("FileCreationDate", fileCreationDate.ToString());

            FileInfo fi = new FileInfo(path);
            double fileSize = fi.Length;
            fileInformation.Add("FileSize", fileSize.ToString());

            return fileInformation;
        }

        public static byte[] GetBytes(string path)
        {
            byte[] output = null;
            try
            {
                output = File.ReadAllBytes(path);
            }
            catch(IOException e)
            {
                Console.WriteLine("this file does not exist\nmethod will return null");
            }
            return output;
            
        }
    }
}

