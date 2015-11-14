using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Client
{
    internal class ClientTcpWorker
    {
        string path { get; set; }
        TcpClient client { get; set; }

        public ClientTcpWorker(string path, int port, string ipAddr)
        {
            this.path = path;
            this.client = new TcpClient(ipAddr, port);
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

        private Dictionary<string, object> CreateDictToSendId(string id)
        {
            Dictionary<string, object> sendingContent = new Dictionary<string, object>();
            sendingContent.Add("Command", "Upload");
            sendingContent.Add("Id", id);
            return sendingContent;
        }

        public void SendIdDict(string id)
        {
            NetworkStream netStream = client.GetStream();
            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, CreateDictToSendId(id));
                byte[] data = ms.ToArray();
                netStream.Write(data, 0, data.Length);
                netStream.Flush();
                netStream.Close();
            }
        }

        public void SendFileDict()
        {
            NetworkStream netStream = client.GetStream();
            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, CreateDictToSendFile());
                //Console.WriteLine("Размер отправляемых данных: {0}", ms.ToArray().Length);
                MessageBox.Show(string.Format("Размер отправляемых данных: {0}", ms.ToArray().Length));

                byte[] data = ms.ToArray();
                netStream.Write(data, 0, data.Length);
                netStream.Flush();
                netStream.Close();
            }

        }
    }
}

