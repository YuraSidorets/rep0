using System;


namespace DB_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpWorker tcpW = new TcpWorker();
            tcpW.Listen();
            
            Console.ReadKey();
        }
    }
}
