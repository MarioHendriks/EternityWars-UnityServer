using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternityWebsocket.Models.WsMessages
{
    public class WsMessage
    {
        public string Action { get; set; }
        public object Content { get; set; }

        public WsMessage()
        {
        }
    }
}
