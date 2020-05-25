 using EternityWebsocket.Models.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternityWebsocket.Models.DTO
{
    abstract class AccountDTO
    {
        public string email { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public AccountStatus accountStatus { get; set; }

        public AccountDTO()
        {

        }
    }
}
