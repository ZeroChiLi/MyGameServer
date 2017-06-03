using System;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MyGameServer.Logic;
using Common.Code;

namespace MyGameServer
{
    public class MyClientPeer : ClientPeer
    {
        //帐号处理对象
        private AccountHandler accountHandler;
        //聊天处理对象
        private ChatHandler chatHandler;

        //客户端连接
        public MyClientPeer(InitRequest initRequest) : base(initRequest)
        {
            accountHandler = new AccountHandler();
            chatHandler = new ChatHandler();
        }

        //客户端断开时调用
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            chatHandler.OnDisconnect(this);
            accountHandler.OnDisconnect(this);
        }

        //客户端发来的请求
        protected override void OnOperationRequest(OperationRequest request, SendParameters sendParameters)
        {
            //按照共同规定的操作码来处理信息
            switch((OpCode)request.OperationCode)
            {   
                //操作用户组
                case OpCode.Account:
                    accountHandler.OnRequest(this, (byte)request.Parameters[80], request);
                    break;
                //操作聊天室
                case OpCode.Chat:
                    chatHandler.OnRequest(this, (byte)request.Parameters[80], request);
                    break;
            }

        }
    }
}
