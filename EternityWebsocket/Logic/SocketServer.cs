using EternityWebsocket.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EternityWebsocket.Models.Logic
{
    class SocketServer
    {
        IPHostEntry ipHostEntry { get; set; }
        IPAddress ipAddress { get; set; }
        IPEndPoint localEndPoint { get; set; }
        Socket server { set; get; }

        public SocketServer(string host, int port)
        {
            ipHostEntry = Dns.GetHostEntry(host);
            ipAddress = ipHostEntry.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, port);
        }

        public void start()
        {
            try
            {

                // Create a Socket that will use Tcp protocol      
                server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // server Socket must be associated with an endpoint using the Bind method  
                server.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.  
                // We will listen 10 requests at a time  
                server.Listen(10);

               
                Thread t = new Thread(new ThreadStart(StartListening));
                t.Start();

                Thread t2 = new Thread(new ThreadStart(readClientMessages));
                t2.Start();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void readClientMessages()
        {
            MessageHandler messageHandler = new MessageHandler();
            Console.WriteLine("Reading incomming messages...");
            while (true)
            {
                foreach (var socketClient in Program.socketClients.ToList())
                {
                    try
                    {
                        string data = null;
                        byte[] bytes = null;
                        bytes = new byte[1024];
                        int bytesRec = socketClient.socket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        messageHandler.HandleMessage(data, socketClient.socket);
                        Console.WriteLine("Text received : {0}", data);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        private void StartListening()
        {
            Console.WriteLine("Waiting for a connection...");

            while (true)
            {

                Socket client = server.Accept();
                // Incoming data from the client.    
                if (client != null)
                {
                    Program.socketClients.Add(new SocketClient(client));
                    Console.WriteLine(Program.socketClients.Count().ToString());
                   
                }
            }
        }

        public void SendClientMessage(Socket client, string data)
        {
            byte[] msg = Encoding.ASCII.GetBytes(data);
            client.Send(msg);
        }
    }
}
