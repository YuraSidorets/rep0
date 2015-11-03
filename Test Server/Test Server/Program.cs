using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SoketServer server = new SoketServer();
            server.TCP();
        }
    }
}
