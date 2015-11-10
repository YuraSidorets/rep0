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
                    RecieveFileDict(handler);
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
            {
                Console.WriteLine("{0} - {1}", kVP.Key, kVP.Value.ToString());
            }
        }

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

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(listOfDict.ToArray()))
            {
                this.contentOfAddingFile = (Dictionary<string, object>)bf.Deserialize(ms);
            }

            TestDisplay();

            DBWorker.SetValue(contentOfAddingFile);
            Console.WriteLine("Получено байт: {0}", totalBytes);
            netStream.Close();
        }
    }
}

