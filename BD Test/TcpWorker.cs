using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace DB_Test
{
    class TcpWorker
    {
        private Dictionary<string, object> contentOfAddingFile;

        /// <summary>
        /// Listen to incoming data
        /// </summary>
        public void Listen()
        {

            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                    Socket handler = sListener.Accept();
                    //RecieveFileDict(handler);
                    RecieveFileDictAndCreateTempFile(handler);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
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
        /// Display recieved dictionary. Test method.
        /// </summary>
        private void TestDisplay()
        {
            foreach (KeyValuePair<string, object> kVP in contentOfAddingFile)
<<<<<<< HEAD

            IPAddress ipAddr = IPAddress.Parse("193.161.14.47");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 5050);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
=======
            foreach (KeyValuePair<string, object> kVP in contentOfAddingFile)
>>>>>>> b2cc0538fe7e60e6ab5705a0c05d657dbdefdd74
            {
                Console.WriteLine("{0} - {1}", kVP.Key, kVP.Value.ToString());
            }
        }

<<<<<<< HEAD
                    byte[] bytes = new byte[1024];
                    //только 1440 байт
                    int bytesRec = handler.Receive(bytes,SocketFlags.None);
                    
                    Console.WriteLine("Размер полученного пакета: {0}", bytesRec);

                    BinaryFormatter bf = new BinaryFormatter();
                   
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(bytes, 0, bytes.Length);

                     
                       
                      // var o = (Dictionary<string, object>)bf.Deserialize(ms);
                        //this.contentOfAddingFile = (Dictionary<string, object>)o;
                    }
=======
        /// <summary>
        /// Recieve Dictionary of (string, object) that contains all info about recieved file 
        /// </summary>
        /// <param name="reciever">Socket of transfer</param>
        private void RecieveFileDict(Socket reciever)
        {
            NetworkStream netStream = new NetworkStream(reciever);

            byte[] data = new byte[1024];
            int dataCitit;
            int totalBytes = 0;
            List<byte> listOfDict = new List<byte>();
            while ((dataCitit = netStream.Read(data, 0, data.Length)) > 0)
            {
                listOfDict.AddRange(data);
                totalBytes += dataCitit;
            }
            using (MemoryStream ms = new MemoryStream(listOfDict.ToArray()))
            BinaryFormatter bf = new BinaryFormatter();
                this.contentOfAddingFile = (Dictionary<string, object>)bf.Deserialize(ms);
>>>>>>> b2cc0538fe7e60e6ab5705a0c05d657dbdefdd74

            TestDisplay();

            DBWorker.SetValue(contentOfAddingFile);
            Console.WriteLine("Получено байт: {0}", totalBytes);
            netStream.Close();
        }

        private void RecieveFileDictAndCreateTempFile(Socket reciever)
        {
            NetworkStream netStream = new NetworkStream(reciever);

            FileStream fs = new FileStream("$temp", FileMode.Create, FileAccess.Write);
            byte[] data = new byte[1024];
            int dataCitit;
            int totalBytes = 0;

            while ((dataCitit = netStream.Read(data, 0, data.Length)) > 0)
            {
                fs.Write(data, 0, dataCitit);
                totalBytes += dataCitit;
            }

            Console.WriteLine("Получено байт: {0}", totalBytes);
            netStream.Close();
            fs.Close();

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes("$temp")))
            {
                this.contentOfAddingFile = (Dictionary<string, object>)bf.Deserialize(ms);
            }
            DBWorker.SetValue(contentOfAddingFile);
        }
    }
}

