using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DB_Test
{
    class TcpWorker
    {
        public Dictionary<string, object> contentOfAddingFile;

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

                    byte[] bytes = new byte[1024];
                    handler.Receive(bytes);
                    
                    string reply = "Запрос, длинной " + bytes.Length.ToString()
                            + " байт получен";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    //if (data.IndexOf("<TheEnd>") > -1)
                    //    handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();
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

        public void TestListen()
        {

            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[10485760];
                    //только 1440 байт
                    int bytesRec = handler.Receive(bytes, 0, 65070, SocketFlags.None);

                    Console.WriteLine("Размер полученного пакета: {0}", bytesRec);

                    BinaryFormatter bf = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);
                        object o = bf.Deserialize(ms);
                        this.contentOfAddingFile = (Dictionary<string, object>)o;
                    }

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // Показываем данные на консоли
                    Console.Write("Полученный текст: " + data + "\n\n");

                    // Отправляем ответ клиенту\
                    string reply = "Спасибо за запрос в " + data.Length.ToString()
                            + " символов";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                        break;
                    }

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

        private void Display()
        {
            foreach (KeyValuePair<string, object> kVP in contentOfAddingFile)
            {
                Console.WriteLine("{0} - {1}", kVP.Key, kVP.Value.ToString());
            }
        }

    }
}

