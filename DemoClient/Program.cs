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
                OpenFileDialog opd = new OpenFileDialog();
                opd.ShowDialog();
                string path = opd.FileName;

                RequestManager rm = new RequestManager(path, 29250);
                rm.SendRequest(RequestEnum.Add);
                
            }

        }
    }
}
