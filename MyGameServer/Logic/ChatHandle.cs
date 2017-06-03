using System.Collections.Generic;
using Photon.SocketServer;
using Common.Code;
using MyGameServer.Cache;
using Code.Common;
using Common.Dto;
using MyGameServer.Model;

namespace MyGameServer.Logic
{
    public class ChatHandle : IHandler
    {
        ChatCache cache { get { return Factory.chatCache; } }

        public void OnDisconnect(MyClientPeer client)
        {
            Leave(client);
        }

        //发出请求
        public void OnRequest(MyClientPeer client, byte subCode, OperationRequest request)
        {
            switch ((ChatCode)subCode)
            {
                case ChatCode.Enter:
                    Enter(client);
                    break;
                case ChatCode.Talk:
                    string contentString = request.Parameters[0].ToString();
                    Talk(client, contentString);
                    break;
            }
        }

        //客户端进入房间
        private void Enter(MyClientPeer client)
        {
            OperationResponse response = new OperationResponse((byte)OpCode.Chat, new Dictionary<byte, object>());
            AccountModel account = Factory.accountCache.GetAccountModel(client);
            RoomModel room = cache.Enter(client, account);

            //房间获取失败
            if (room == null)
            {
                SendResponseWithInformation(client, response, "进入房间失败", -1);
                return;
            }

            //进入成功，创建房间模型
            RoomDto roomDto = new RoomDto();
            foreach (var item in room.clientAccountDict.Values)
                roomDto.accountList .Add(new AccountDto() { Account = item.Account, Password = item.Password });
            response.Parameters[80] = ChatCode.Enter;
            response.Parameters[0] = LitJson.JsonMapper.ToJson(roomDto);
            SendResponseWithInformation(client, response, "进入房间成功", 0);

            //告诉其他房间客户端
            AccountDto accountDto = new AccountDto() { Account = account.Account, Password = account.Password };
            response.Parameters[80] = ChatCode.Add;
            response.Parameters[0] = LitJson.JsonMapper.ToJson(accountDto);
            foreach (var item in room.clientAccountDict.Keys)
            {
                if (item == client)
                    continue;
                SendResponseWithInformation(item, response, "新的客户端进入房间" , 0);
            }

        }

        //客户端离开房间
        private void Leave(MyClientPeer client)
        {
            AccountModel accountModel = cache.Leave(client);
            if (accountModel == null)
                return;
            AccountDto accountDto = new AccountDto() { Account = accountModel.Account, Password = accountModel.Password };

            OperationResponse response = new OperationResponse((byte)OpCode.Chat, new Dictionary<byte, object>());
            response.Parameters[80] = ChatCode.Leave;
            response.Parameters[0] = LitJson.JsonMapper.ToJson(accountDto);
            RoomModel room = cache.GetRoomModel();
            //群发房间其他人有人离开
            foreach (var item in room.clientAccountDict.Keys)
            {
                SendResponseWithInformation(item, response,string.Format("用户 {0} 离开房间",accountDto.Account),0);
            }
        }

        //聊天
        private void Talk(MyClientPeer client, string contentStr)
        {
            OperationResponse response = new OperationResponse((byte)OpCode.Chat, new Dictionary<byte, object>());
            response.Parameters[80] = ChatCode.Talk;
            AccountModel account = Factory.accountCache.GetAccountModel(client);
            RoomModel room = cache.GetRoomModel();

            response.Parameters[0] = string.Format("{0} : {1}", account.Account, contentStr);

            foreach (var item in room.clientAccountDict.Keys)
                SendResponseWithInformation(item, response);
        }


        //发送信息
        private void SendResponseWithInformation(MyClientPeer client, OperationResponse response, string debugMessage = "", short returnCode = 0)
        {
            response.DebugMessage = debugMessage;
            response.ReturnCode = returnCode;
            client.SendOperationResponse(response, new SendParameters());
        }
    }
}