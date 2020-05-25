using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EternityWebsocket.Api.Services
{
    [ServiceContract]
    interface IGameService
    {
            [OperationContract]
            [WebInvoke(Method = "POST",
                 ResponseFormat = WebMessageFormat.Json,
                 BodyStyle = WebMessageBodyStyle.Wrapped,
                 UriTemplate = "startGame")]
            [return: MessageParameter(Name = "Data")]
        bool startGame(Stream body);
    }
}
