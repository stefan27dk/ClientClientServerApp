using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ClientClientServerApp
{
    class Program
    {

        public static NetworkStream stm;
        public static BinaryReader reader;
        public static BinaryWriter writer;


        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");
                tcpclnt.Connect("127.0.0.1", 8001);     // use the ipaddress as
                                                        // in the server program
                Console.WriteLine("Connected");
                stm = tcpclnt.GetStream();
                reader = new BinaryReader(stm);
                writer = new BinaryWriter(stm);

                // Lidt Nyt kode til klienten
                Thread t = new Thread(new ThreadStart(VisInput));
                t.Start();
                string str = "s";
                while (str != "")
                {
                    Console.Write("Enter the string to be transmitted : ");
                    str = Console.ReadLine();
                    Console.WriteLine("Transmitting.....");
                    writer.Write(str);
                    Console.WriteLine("data sent...");
                    //nedenstaaende lжseoperationer er flyttet til en seperat
                    //traad der haandterer lesning fra ALLE klienterne
                    //string txt = reader.ReadString();
                    //Console.WriteLine(txt + " received");
                }
                tcpclnt.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        public static void VisInput()
        {
            string txt = "";
            while (true)
            {
                try
                {
                    txt = reader.ReadString();
                    Console.WriteLine(txt);
                }
                catch
                {
                    Console.WriteLine("Logger af...");
                    break;
                }
            }
        }


    }
}
