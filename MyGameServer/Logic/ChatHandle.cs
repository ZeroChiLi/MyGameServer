using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        ChatCache cache { get { return Factory.ChatCache; } }

        public void OnDisconnect(MyClientPeer client)
        {
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
                    break;
            }
        }

        //客户端进入房间
        private void Enter(MyClientPeer client)
        {
            OperationResponse response = new OperationResponse((byte)OpCode.Chat, new Dictionary<byte, object>());
            AccountModel account = Factory.AccountCache.GetModel(client);
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
            {
                roomDto.accountDict.Add(new AccountDto() { Account = item.Account, Password = item.Password });
            }
            response.Parameters[0] = LitJson.JsonMapper.ToJson(roomDto);
            SendResponseWithInformation(client, response, "进入房间成功", 1);

            //告诉其他房间客户端
            AccountDto accountDto = new AccountDto() { Account = account.Account, Password = account.Password };
            response.Parameters[0] = LitJson.JsonMapper.ToJson(accountDto);
            foreach(var item in room.clientAccountDict.Keys)
            {
                SendResponseWithInformation(item, response, "新的客户端进入房间", 2);
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
