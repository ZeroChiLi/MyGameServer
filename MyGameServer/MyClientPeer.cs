using System;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MyGameServer
{
    public class MyClientPeer : ClientPeer
    {
        public MyClientPeer(InitRequest initRequest) : base(initRequest)
        {
            MyApplication.Log("客户端 MyClientPeer 连接。");
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            MyApplication.Log("客户端断开。 OnDisconnect() ");
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            MyApplication.Log("客户端请求。OnOperationRequest() ");
            MyApplication.Log(operationRequest.Parameters[0].ToString());
            SendOperationResponse(new OperationResponse(), sendParameters);
        }
    }
}
