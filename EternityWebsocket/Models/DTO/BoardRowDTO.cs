using System;
using System.Collections.Generic;

namespace EternityWebsocket.Models.DTO
{
    class BoardRowDTO
    {
       public List<CardSlotDTO> cardSlotList { get; set; }

        public BoardRowDTO()
        {
            cardSlotList = new List<CardSlotDTO>();
            AddSlots(cardSlotList);
        }

        private void AddSlots(List<CardSlotDTO> cardSlotList)
        {
            for (int i = 0; i < 6; i++)
            {
                cardSlotList.Add(new CardSlotDTO());
            }
        }
    }
}