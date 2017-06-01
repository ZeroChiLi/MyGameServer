using System;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MyGameServer.Logic;
using Code.Common;

namespace MyGameServer
{
    public class MyClientPeer : ClientPeer
    {

        AccountHandler account;

        //客户端连接Peer
        public MyClientPeer(InitRequest initRequest) : base(initRequest)
        {
            account = new AccountHandler();
        }

        //客户端断开
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            MyApplication.Log("客户端断开。 OnDisconnect() ");
        }

        //客户端发出请求
        protected override void OnOperationRequest(OperationRequest request, SendParameters sendParameters)
        {
            switch((OpCode)request.OperationCode)
            {
                case OpCode.Account:
                    account.OnClientRequest(this, (byte)request.Parameters[80], request);
                    break;
            }

        }
    }
}
