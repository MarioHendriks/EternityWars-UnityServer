using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EternityWebsocket.Models.DTO;
using Newtonsoft.Json;

namespace EternityWebsocket.Api.Services
{
    class GameRestService : IGameService
    {
        [return: MessageParameter(Name = "Data")]
        public bool startGame(Stream body)
        {

            string data = new StreamReader(body).ReadToEnd();

            try
            {
                if(data != null)
                {
                    GameDTO game = JsonConvert.DeserializeObject<GameDTO>(data);
                    Program.OnGoingGames.Add(game);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return false;
        }
    }
}
