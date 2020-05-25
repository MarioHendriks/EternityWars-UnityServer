using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternityWebsocket.Models.DTO
{
    class CardDTO
    {
        public int cardId { get; set; }
        public string name { get; set; }
        public int health { get; set; }
        public int attack { get; set; }
        public int blue_mana { get; set; }
        public int death_essence { get; set; }
        public bool isSleeping { get; set; }
        public bool taunt { get; set; }

        public CardDTO()
        {
            this.isSleeping = true;
        }
    }
}
