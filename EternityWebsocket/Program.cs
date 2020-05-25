using EternityWebsocket.Models;
using EternityWebsocket.Models.DTO;
using EternityWebsocket.Models.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EternityWebsocket
{
    class Program
    {
        public static SocketServer socketServer;
        public static List<GameDTO> OnGoingGames;
        public static List<SocketClient> socketClients;
        public static int Main(string[] args)
        {
            OnGoingGames = new List<GameDTO>();
            socketClients = new List<SocketClient>();

            Thread t = new Thread(new ThreadStart(StartApiServer));
            t.Start();

            socketServer = new SocketServer("localhost", 9595);
            socketServer.start();

            Console.ReadLine();
            return 0;
        }

        static void StartApiServer()
        {
            WebServiceHost hostWeb = new WebServiceHost(typeof(Api.Services.GameRestService));
            ServiceEndpoint ep = hostWeb.AddServiceEndpoint(typeof(Api.Services.IGameService), new WebHttpBinding(), "");
            ServiceDebugBehavior stp = hostWeb.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = false;
            hostWeb.Open();
            Console.WriteLine("Service Host started @ " + DateTime.Now.ToString());
        }

    }
}
