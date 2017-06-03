using Photon.SocketServer;

namespace MyGameServer.Logic
{
    //客户端逻辑处理抽象类
    public abstract class Handler
    {
        //客户端发起请求，参数：客户端，子操作码，操作请求。
        public abstract void OnRequest(MyClientPeer client, byte subCode, OperationRequest request);

        //客户端下线。
        public abstract void OnDisconnect(MyClientPeer client);

        //发送数据给客户端
        protected void SendResponseToClient(MyClientPeer client, OperationResponse response, string debugMessage = "", short returnCode = 0)
        {
            response.DebugMessage = debugMessage;               //调试信息
            response.ReturnCode = returnCode;                   //返回码
            client.SendOperationResponse(response, new SendParameters());   //发送回复信息
        }
    }
}
