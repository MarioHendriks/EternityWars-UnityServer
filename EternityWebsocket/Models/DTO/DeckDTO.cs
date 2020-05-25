using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternityWebsocket.Models.DTO
{
    class DeckDTO
    {
        public int deckId { get; set; }
        public int userId { get; set; }
        public string name { get; set; }
        public CardCollection cards { get; set; }

        public DeckDTO()
        {

        }
    }
}
