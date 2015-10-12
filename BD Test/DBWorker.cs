//TODO: Убрать консольные команды 
//TODO: Сделать этот клас Singleton'ом (сегодня-завтра займусь. скорее завтра, ибо сегодня футбик :D)
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

        public DBWorker()
        {
            MySqlConnectionStringBuilder mysqlCSB = new MySqlConnectionStringBuilder();
            mysqlCSB.Server = "mysql.sidoretsyura.myjino.ru";
            mysqlCSB.Database = "sidoretsyura_testdb";
            mysqlCSB.UserID = "sidoretsyura";
            mysqlCSB.Password = "123456";
            this.mysqlCSB = mysqlCSB;
        }


        /// <summary>
        /// Inputs a new row with file and iformation about it to DB 
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="table">Table in DB</param>
        public void SetValue(string filePath, string table)
        {

            string queryString = string.Format(@"INSERT INTO {0} ({1},{2},{3},{4},Data) VALUES ('{5}','{6}','{7:yyyy-MM-dd hh:mm:ss}','{8}',?file)",
               table, FileWorker.GetFileInfo(filePath).Keys.ToArray()[0], FileWorker.GetFileInfo(filePath).Keys.ToArray()[1],
                FileWorker.GetFileInfo(filePath).Keys.ToArray()[2], FileWorker.GetFileInfo(filePath).Keys.ToArray()[3],

                FileWorker.GetFileInfo(filePath).Values.ToArray()[0], FileWorker.GetFileInfo(filePath).Values.ToArray()[1],
                FileWorker.GetFileInfo(filePath).Values.ToArray()[2], FileWorker.GetFileInfo(filePath).Values.ToArray()[3]);

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

        /// <summary>
        /// Returns DataTable instance that includes all the values in db except binary data
        /// </summary>
        public DataTable ReadAllValues()
        {
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
                con.ConnectionString = this.mysqlCSB.ConnectionString;
                MySqlCommand com = new MySqlCommand(query, con);
                con.Open();

                using (MySqlDataReader dr = com.ExecuteReader())
                {
                    if (dr.HasRows) dt.Load(dr);
                }

            }
            return dt;
        }

        public object[] GetFileToWrite(int id)
        {
            string query = string.Format(
                      @"SELECT Name,     
                               Type,
                               Data               
                        FROM   new_table 
                        WHERE  Id = {0}", id);

            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = this.mysqlCSB.ConnectionString;
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

