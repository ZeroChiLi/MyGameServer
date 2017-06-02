using System;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MyGameServer.Logic;
using Code.Common;

namespace MyGameServer
{
    public class MyClientPeer : ClientPeer
    {
        //帐号处理
        AccountHandler account;
        //聊天处理
        ChatHandle chat;

        //客户端连接Peer
        public MyClientPeer(InitRequest initRequest) : base(initRequest)
        {
            account = new AccountHandler();
            chat = new ChatHandle();
        }

        //客户端断开
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            chat.OnDisconnect(this);
            account.OnDisconnect(this);
        }

        //客户端发出请求
        protected override void OnOperationRequest(OperationRequest request, SendParameters sendParameters)
        {
            switch((OpCode)request.OperationCode)
            {
                case OpCode.Account:
                    account.OnRequest(this, (byte)request.Parameters[80], request);
                    break;
                case OpCode.Chat:
                    chat.OnRequest(this, (byte)request.Parameters[80], request)
                    break;
            }

        }
    }
}
