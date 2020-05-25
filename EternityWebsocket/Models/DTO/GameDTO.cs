using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;

namespace EternityWebsocket.Models.DTO
{
    class GameDTO
    {
        public int id { get; set; }
        public List<PlayerDTO> connectedPlayers { get; set; }
        public int playerTurn { get; set; }
        public int timer { get; set; }
        public bool victory { get; set; }


        public GameDTO()
        {

        }
    }
}
