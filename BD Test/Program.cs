using System;


namespace DB_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerTcpWorker tcpW = new ServerTcpWorker();
            tcpW.Listen();
            
            Console.ReadKey();
        }
    }
}
