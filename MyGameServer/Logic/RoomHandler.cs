using System.Collections.Generic;
using Photon.SocketServer;
using Common.Code;
using Common.Dto;
using MyGameServer.Cache;
using MyGameServer.Model;

namespace MyGameServer.Logic
{
    //聊天
    public class RoomHandler : Handler
    {
        RoomCache chatCache = Factory.chatCache;

        //服务器回复给客户端的信息
        OperationResponse response = new OperationResponse((byte)OpCode.Room, new Dictionary<byte, object>());


        //断开连接时
        public override void OnDisconnect(MyClientPeer client)
        {
            Leave(client);
        }

        //处理客户端发来的请求
        public override void OnRequest(MyClientPeer client, byte subCode, OperationRequest request)
        {
            switch ((RoomCode)subCode)
            {
                //请求进入房间
                case RoomCode.Enter:
                    Enter(client);
                    break;
                //发出聊天信息
                case RoomCode.Talk:
                    string contentString = request.Parameters[0].ToString();
                    Talk(client, contentString);
                    break;
            }
        }

        //客户端进入房间
        private void Enter(MyClientPeer client)
        {
            AccountModel account = Factory.accountCache.GetAccountModel(client);
            RoomModel room = chatCache.Enter(client, account);

            //房间获取失败
            if (room == null)
            {
                SendResponseToClient(client, response, "进入房间失败", -1);
                return;
            }

            //进入成功，创建房间模型
            RoomDto roomDto = new RoomDto();
            //获取当前房间所有客户端，发送给客户端
            foreach (var item in room.clientAccountDict.Values)
                roomDto.AccountList .Add(new AccountDto() { AccountName = item.AccountName, Password = item.Password });
            response.Parameters[80] = RoomCode.Enter;
            response.Parameters[0] = LitJson.JsonMapper.ToJson(roomDto);
            SendResponseToClient(client, response, "进入房间成功", 0);

            //把这个新来的用户信息告诉房间内的其他客户端
            AccountDto accountDto = new AccountDto() { AccountName = account.AccountName, Password = account.Password };
            response.Parameters[80] = RoomCode.Add;
            response.Parameters[0] = LitJson.JsonMapper.ToJson(accountDto);
            foreach (var item in room.clientAccountDict.Keys)
            {
                if (item == client)     //除了他自己不发送
                    continue;
                SendResponseToClient(item, response, "新的客户端进入房间" , 0);
            }

        }

        //客户端离开房间
        private void Leave(MyClientPeer client)
        {
            //获取离开房间用户的信息
            AccountModel accountModel = chatCache.Leave(client);
            if (accountModel == null)
                return;
            //创建Dto以用来传输数据
            AccountDto accountDto = new AccountDto() { AccountName = accountModel.AccountName, Password = accountModel.Password };

            response.Parameters[80] = RoomCode.Leave;
            response.Parameters[0] = LitJson.JsonMapper.ToJson(accountDto);
            RoomModel room = chatCache.GetRoomModel();
            //群发给房间其他人，自己要离开
            foreach (var item in room.clientAccountDict.Keys)
                SendResponseToClient(item, response,string.Format("用户 {0} 离开房间",accountDto.AccountName),0);
        }

        //客户端发出聊天信息
        private void Talk(MyClientPeer client, string contentStr)
        {
            AccountModel accountModel = Factory.accountCache.GetAccountModel(client);
            RoomModel room = chatCache.GetRoomModel();

            response.Parameters[80] = RoomCode.Talk;
            response.Parameters[0] = string.Format("{0} : {1}", accountModel.AccountName, contentStr);
            foreach (var item in room.clientAccountDict.Keys)
                SendResponseToClient(item, response);
        }
    }
}