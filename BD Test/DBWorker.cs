//TODO: Убрать консольные команды 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DB_Test
{
    class DBWorker
    {
        private MySqlConnectionStringBuilder mysqlCSB;
       

        public DBWorker(MySqlConnectionStringBuilder MYSQlcsb)
        {
            mysqlCSB = MYSQlcsb;
        }

        /// <summary>
        /// Inputs a new row with file and iformation about it to DB 
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="table">Table in DB</param>
        public void SetValue(string filePath, string table)
        {
            Dictionary<string, object> info = FileWorker.GetFileInfo(filePath);

            string queryString = string.Format(@"INSERT INTO {0} ({1},{2},{3},{4},Data) VALUES ('{5}','{6}','{7:yyyy-MM-dd hh:mm:ss}','{8}',?file)",

                table,info.Keys.ToArray()[0], info.Keys.ToArray()[1], 
                info.Keys.ToArray()[2], info.Keys.ToArray()[3],

                info.Values.ToArray()[0], info.Values.ToArray()[1],
                info.Values.ToArray()[2], info.Values.ToArray()[3]);

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mysqlCSB.ConnectionString;

                MySqlCommand mainCommand = new MySqlCommand(queryString, con);
                MySqlCommand timeoutCommand = new MySqlCommand("set net_write_timeout = 99999; set net_read_timeout = 99999", con);

                try
                {
                    byte[] data = FileWorker.GetBytes(filePath);

                    MySqlParameter param = new MySqlParameter("?file", MySqlDbType.LongBlob, data.Length);
                    param.Value = data;
                    mainCommand.Parameters.Add(param);
                                     
                    con.Open();
                    timeoutCommand.ExecuteNonQuery();
                    mainCommand.CommandText = queryString;
                    mainCommand.ExecuteNonQuery();

                    con.Close();
                    mainCommand.Dispose();
                }
                catch (Exception e)
                {
                   //!!!!!!!!!
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
        }


        public void GetData(string column, string table)
        {
            throw new NotImplementedException();

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

            throw new NotImplementedException();
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
    /// <summary>
    /// Gets information about file
    /// </summary>
    static class FileWorker
    {
        public static Dictionary<string, object> GetFileInfo(string path)
        {
            Dictionary<string, object> fileInformation = new Dictionary<string, object>();
            string fileName = Path.GetFileNameWithoutExtension(path);
            fileInformation.Add("Name", fileName);

            string fileType = Path.GetExtension(path);
            fileInformation.Add("Type", fileType);

            DateTime fileCreationDate = File.GetCreationTime(path);
            fileInformation.Add("Date", fileCreationDate);

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
                ///!!!!!!!!!!
                Console.WriteLine("this file does not exist\n method will return null\n",e);
            }
            return output;    
        }
    }
}

