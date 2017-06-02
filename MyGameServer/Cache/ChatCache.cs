using MyGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGameServer.Cache
{
    public class ChatCache
    {
        private RoomModel room;

        public ChatCache()
        {
            room = new RoomModel(0);
        }

        //进入房间模型
        public RoomModel Enter(MyClientPeer client, AccountModel model)
        {
            if (room.Contains(client))
                return null;

            room.Add(client, model);
            return room;
        }

        public AccountModel Leave(MyClientPeer client)
        {
            AccountModel model = null;
            room.clientAccountDict.TryGetValue(client,out model);
            return model;
        }

        //获取房间模型
        public RoomModel GetRoomModel()
        {
            return room;
        }
    }
}
