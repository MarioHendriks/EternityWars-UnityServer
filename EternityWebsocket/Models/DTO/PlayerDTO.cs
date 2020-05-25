using EternityWebsocket.Models.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternityWebsocket.Models.DTO
{
    class PlayerDTO : AccountDTO 
    {
        public DeckDTO deck { get; set; }
        public BoardRowDTO boardrow { get; set; }
        public List<CardDTO> cardsInHand { get; set; }
        public List<CardDTO> cardsInDeck { get; set; }
        public LobbyPlayerStatus lobbyPlayerStatus { get; set; }
        public HeroDTO hero { get; set; }

        public PlayerDTO()
        {

        }
    }
}
