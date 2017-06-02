using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using Common.Code;
using MyGameServer.Cache;
using Code.Common;

namespace MyGameServer.Logic
{
    public class ChatHandle : IHandler
    {
        ChatCache cache { get { return Factory.ChatCache; } }

        public void OnDisconnect(MyClientPeer client)
        {
        }

        //发出请求
        public void OnRequest(MyClientPeer client, byte subCode, OperationRequest request)
        {
            switch((ChatCode)subCode)
            {
                case ChatCode.Enter:
                    break;
                case ChatCode.Talk:
                    break;
            }
        }

        //客户端进入房间
        private void Enter(MyClientPeer client)
        {
            OperationResponse response = new OperationResponse((byte)OpCode.Chat, new Dictionary<byte, object>());

            if (!cache.Enter(client,Factory.AccountCache.GetModel(client)))
            {
                SendResponseWithInformation(client, response, "进入房间失败", -1);
                return;
            }


        }

        private void SendResponseWithInformation(MyClientPeer client, OperationResponse response, string debugMessage = "", short returnCode = 0)
        {
            response.DebugMessage = debugMessage;
            response.ReturnCode = returnCode;
            client.SendOperationResponse(response, new SendParameters());
        }
    }
}
