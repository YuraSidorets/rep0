using System;
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
                Console.WriteLine("1 : Upload File\n2 : DownloadFile");
                string selector = Console.ReadLine();

                switch(selector)
                {
                    case "1":
                        {
                            OpenFileDialog opd = new OpenFileDialog();
                            opd.ShowDialog();
                            string path = opd.FileName;

                            ClientTcpWorker rm = new ClientTcpWorker(path, 5050, "::1");
                            rm.SendFileDict();
                            break;
                        }

                    case "2":
                        {
                            Console.WriteLine("Id of file to download");
                            string id = Console.ReadLine();

                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.ShowDialog();
                            string path = sfd.FileName;

                            ClientTcpWorker rm = new ClientTcpWorker(path, 5050, "::1");
                            rm.SendIdDict(id);
                            break;
                        }
                }


                
            }

        }
    }
}
