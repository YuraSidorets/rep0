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
            TcpWorker tcpW = new TcpWorker();
            tcpW.TestListen();
            Console.ReadKey();
        }
    }
}
