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
    static class DBWorker
    {
        private static MySqlConnectionStringBuilder mysqlCSB;

        /// <summary>
        /// Init static field mysqlCSB
        /// </summary>
        private static void Init()
        {
            if(mysqlCSB == null)
            {
                MySqlConnectionStringBuilder CSB = new MySqlConnectionStringBuilder();
                CSB.Server = "mysql.sidoretsyura.myjino.ru";
                CSB.Database = "sidoretsyura_testdb";
                CSB.UserID = "sidoretsyura";
                CSB.Password = "123456";
                mysqlCSB = CSB;
            }
        }


        /// <summary>
        /// Inputs a new row with file and iformation about it to DB 
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="table">Table in DB</param>
        public static void SetValue(string filePath, string table)
        {

            Init();
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


        /// <summary>
        /// Returns DataTable instance that includes all the values in db except binary data
        /// </summary>
        public static DataTable ReadAllValues()
        {
            Init();

            string query = @"SELECT Id, 
                               Name,     
                               Type,
                               Date,
                               Size,
                               Description               
                        FROM   new_table 
                        WHERE  Id > 5";
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mysqlCSB.ConnectionString;
                MySqlCommand com = new MySqlCommand(query, con);                         
                con.Open();

                using (MySqlDataReader dr = com.ExecuteReader())
                {
                    if (dr.HasRows) dt.Load(dr);
                }

            }
            return dt;
        }

        public static object[] GetFileToWrite(int id)
        {
            Init();

            string query = string.Format(
                      @"SELECT Name,     
                               Type,
                               Data               
                        FROM   new_table 
                        WHERE  Id = {0}", id);

            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = mysqlCSB.ConnectionString;
                MySqlCommand com = new MySqlCommand(query, con);
                con.Open();

                using (MySqlDataReader dr = com.ExecuteReader())
                {
                    if (dr.HasRows) dt.Load(dr);
                }

            }
            return dt.Rows[0].ItemArray;
        }
    }
}

