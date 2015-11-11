using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DemoClient
{
    class RequestManager
    {
        string path { get; set; }
        int port { get; set; } //11000
        //public string SendingFilePath = string.Empty;
        private const int BufferSize = 1024;

        public RequestManager(string path, int port)
        {
            this.path = path;
            this.port = port;
        }

        public void SendRequest(RequestEnum req)
        {
            try
            {
                switch (req)
                {
                    case RequestEnum.Add:
                        {
                            //this.SendRequestToAddFile();
                            this.SendFileDict();
                            break;
                        }

                    case RequestEnum.Download:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }


        /// <summary>
        /// Method sends string command to server
        /// </summary>
        private void SendRequestToAddFile()
        {
            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[0];
            var ipEndPoint = new IPEndPoint(ipAddr, port);
            var sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(ipEndPoint);
            Console.WriteLine("Сокет соединяется с {0} ", sender.RemoteEndPoint.ToString());

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, CreateDictToSendFile());

                Console.WriteLine("Размер отправляемых данных: {0}", ms.ToArray().Length);
                
                sender.Send(ms.ToArray(), 0, (int)ms.Length, SocketFlags.None);
            }
            //sender.SendFile(path);

            FileInfo fi = new FileInfo(path);
            Console.WriteLine("Размер отправляемых данных: {0}", fi.Length);

            byte[] bytesOfAnswer = new byte[10485760];
            int bytesRec = sender.Receive(bytesOfAnswer);
            string answer = Encoding.UTF8.GetString(bytesOfAnswer, 0, bytesRec);
            Console.WriteLine("\nОтвет от сервера: {0}\n\n", answer);
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

        }

        private Dictionary<string, object> CreateDictToSendFile()
        {
            Dictionary<string, object> fileInformation = new Dictionary<string, object>();
            fileInformation.Add("Command", "Add");
            fileInformation.Add("Name", FileWorkerCl.GetFileInfo(path)["Name"]);
            fileInformation.Add("Type", FileWorkerCl.GetFileInfo(path)["Type"]);
            fileInformation.Add("Date", FileWorkerCl.GetFileInfo(path)["Date"]);
            fileInformation.Add("Size", FileWorkerCl.GetFileInfo(path)["Size"]);
            fileInformation.Add("Data", FileWorkerCl.GetBytes(path));
            fileInformation.Add("Description", string.Empty); //!!!
            return fileInformation;
        }

        void SendFileDict()
        {
            TcpClient client = new TcpClient("::1", 11000);
            
            NetworkStream netStream = client.GetStream();
            //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            //byte[] data = new byte[fs.Length];
            //List<byte> data = new List<byte>();
            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, CreateDictToSendFile());
                Console.WriteLine("Размер отправляемых данных: {0}", ms.ToArray().Length);

                byte[] data = ms.ToArray();
                netStream.Write(data, 0, data.Length);
                netStream.Flush();
                netStream.Close();
            }

            //fs.Read(data, 0, data.Length);
            //fs.Flush();
            //fs.Close();


    }
}
}

