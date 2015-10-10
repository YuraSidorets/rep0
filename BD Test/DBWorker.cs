﻿//TODO: Убрать консольные команды 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace DB_Test
{
    class DBWorker
    {
        private MySqlConnectionStringBuilder mysqlCSB;


        private DBWorker()
        {

        }

        public DBWorker(MySqlConnectionStringBuilder MYSQlcsb)
        {
            mysqlCSB = MYSQlcsb;
        }


        public void SetValue(string filePath, string table)
        {
            string queryString = String.Format(@"INSERT INTO {0} ({1},{2},{3},{4}) VALUES ('{5}','{6}','{7}','{8}')",

                table,FileWorker.GetFileInfo(filePath).Keys.ToArray()[0], FileWorker.GetFileInfo(filePath).Keys.ToArray()[1], 
                FileWorker.GetFileInfo(filePath).Keys.ToArray()[2], FileWorker.GetFileInfo(filePath).Keys.ToArray()[3],

                FileWorker.GetFileInfo(filePath).Values.ToArray()[0], FileWorker.GetFileInfo(filePath).Values.ToArray()[1],
                FileWorker.GetFileInfo(filePath).Values.ToArray()[2], FileWorker.GetFileInfo(filePath).Values.ToArray()[3]);

            MySqlParameter sqlParameter = new MySqlParameter("@file", SqlDbType.VarBinary);

            Console.WriteLine();
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mysqlCSB.ConnectionString;
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {

                    MemoryStream a = new MemoryStream(File.ReadAllBytes(filePath), false);
                    
                    sqlParameter.Value = a.ToArray();
                    com.Parameters.Add(sqlParameter);

                    con.Open();
                    com.CommandText = queryString;
                    com.ExecuteNonQuery();
                    con.Close();
                    com.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
        }


        public void GetData(string column, string table)
        {
            string queryString = String.Format(@"SELECT {0} FROM {1}", column, table);

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
                            Console.WriteLine("{0}\t", reader.GetValue(0));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
        }

        public void DelValue()
        {
            string queryString = String.Format(@"");

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
                            Console.WriteLine("{0}\t", reader.GetValue(0));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }

        }         
    }

    static class FileWorker
    {
        public static Dictionary<string, string> GetFileInfo(string path)
        {
            Dictionary<string, string> fileInformation = new Dictionary<string, string>();
            string fileName = Path.GetFileNameWithoutExtension(path);
            fileInformation.Add("Name", fileName);

            string fileType = Path.GetExtension(path);
            fileInformation.Add("Type", fileType);

            DateTime fileCreationDate = File.GetCreationTime(path);
            fileInformation.Add("Date", fileCreationDate.ToString());

            FileInfo fi = new FileInfo(path);
            double fileSize = fi.Length;
            fileInformation.Add("Size", fileSize.ToString());

            return fileInformation;
        }

        public static byte[] GetBytes(string path)
        {
            byte[] output = null;
            try
            {
                output = File.ReadAllBytes(path);
            }
            catch (IOException e)
            {
                Console.WriteLine("this file does not exist\n method will return null");
            }
            return output;    
        }
    }
}
