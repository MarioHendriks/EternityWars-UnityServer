using EternityWebsocket.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EternityWebsocket.Models
{
    class SocketClient
    {
        public Socket socket { get; set; }
        public UserDTO userDTO { get; set; }

        public SocketClient()
        {

        }

        public SocketClient(Socket socket)
        {
            this.socket = socket;
        }
    }
}
