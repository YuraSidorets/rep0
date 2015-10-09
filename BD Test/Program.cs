using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> dict = GetDBInf.GetFileInfo(@"E:\1.txt");
            foreach (KeyValuePair<string,string> pair in dict)
            {
                Console.WriteLine("{0}--{1}", pair.Key, pair.Value);
            }
            Console.WriteLine(new string('-', 20));

            byte[] b = GetDBInf.GetBytes(@"E:\1.txt");
            foreach (byte bt in b)
            {
                Console.Write(bt);
            }
            Console.ReadKey();
        }
        
    }
}
