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
    class RegisterLogic
    {
        public SocketClient GetSocketClient(Socket socket)
        {
            foreach (SocketClient socketClient in Program.socketClients)
            {
                if (socketClient.socket == socket)
                {
                    return socketClient;
                }
            }
            return null;
        }


        public void RegisterClient(SocketClient socketClient, Socket socket, Object user)
        {

            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(JsonConvert.SerializeObject(user));

            foreach (SocketClient sc in Program.socketClients)
            {
                if (sc == socketClient)
                {
                    sc.userDTO = userDTO;
                    sc.socket = socket;
                }
            }
        }
    }
}
