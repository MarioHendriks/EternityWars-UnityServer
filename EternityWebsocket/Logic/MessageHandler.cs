using EternityWebsocket.Models;
using EternityWebsocket.Models.DTO;
using EternityWebsocket.Models.WsMessages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EternityWebsocket.Logic
{
    class MessageHandler
    {
        public void HandleMessage(string message, Socket socket)
        { 
            WsMessage wsMessage = JsonConvert.DeserializeObject<WsMessage>(message);


            switch (wsMessage.Action)
            {
                default:
                    break;
                case "Register":
                    RegisterLogic registerLogic = new RegisterLogic();

                    SocketClient socketClient = registerLogic.GetSocketClient(socket);

                    registerLogic.RegisterClient(socketClient, socket, wsMessage.Content);

                    Console.WriteLine(Program.socketClients.Count().ToString());
                    break;
            }
        }
    }
}
