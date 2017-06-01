using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;

namespace MyGameServer.Logic
{
    public interface IHandler
    {
        //客户端发起请求。
        void OnClientRequest(MyClientPeer client, byte subCode, OperationRequest request);

        //客户端下线。
        void OnClientOffline(MyClientPeer client);
    }
}
