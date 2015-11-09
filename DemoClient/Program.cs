using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            while (true)
            {
                //RequestManager.SendRequest("::1", 11000, Encoding.UTF8.GetBytes("init"), RequestEnum.Init);
                //Console.WriteLine("Message: ");
                //string message = Console.ReadLine();
                //byte[] byteMessage = Encoding.UTF8.GetBytes(message);
                OpenFileDialog opd = new OpenFileDialog();
                opd.ShowDialog();
                string path = opd.FileName;

                RequestManager rm = new RequestManager(path, 11000);
                rm.SendRequest(RequestEnum.Add);
            }

        }
    }
}
