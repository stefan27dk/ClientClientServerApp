using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ClientClientServerApp
{
    class Program
    {

        public static NetworkStream NetStream;
        public static BinaryReader Reader;
        public static BinaryWriter Writer;


        static void Main(string[] args)
        {
            try
            {
                // Connection-Settings--------------------------------------
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");
                tcpclnt.Connect("127.0.0.1", 8001);  // Server IP - Adress                   
                Console.WriteLine("Connected");

                // Stream-Settings-----------------------------------------------
                NetStream = tcpclnt.GetStream();

                // Initialize Reader & Writer for the stream
                Reader = new BinaryReader(NetStream);
                Writer = new BinaryWriter(NetStream);

                // Listener for Server -- Talking to Client
                new Thread(()=> ListenForServerOutput()).Start();
                 
                string clientBid = "No";
                while (true)
                {
                    Console.Write("Enter Bid: ");
                    clientBid = Console.ReadLine();
                    
                    Console.WriteLine(Environment.NewLine + "Sending..... ");
                    if(!string.IsNullOrWhiteSpace(clientBid))
                    { 
                     Writer.Write(clientBid); // Send To Server
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input");
                    }
                    Console.WriteLine("Data Sent" + Environment.NewLine);     
                }  

                tcpclnt.Close();
            }
            catch (Exception e)
            {
                Environment.Exit(0);
            }
        }


        // Server Output -- Server Talking to Client--||Method||-------------------------------------------------------
        public static void ListenForServerOutput()
        {
            string txt = "";
            while (true)
            {
                try
                {
                    txt = Reader.ReadString(); // Read Server Output to Client
                    Console.WriteLine(txt);
                }
                catch
                {
                    Console.WriteLine("Closing...");
                    break;
                }
            }
        }


    }
}
