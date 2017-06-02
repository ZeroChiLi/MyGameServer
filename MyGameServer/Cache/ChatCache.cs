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

        //进入房间
        public RoomModel Enter(MyClientPeer client, AccountModel model)
        {
            if (room.Contains(client))
                return null;

            room.Add(client, model);
            return room;
        }

        //离开房间
        public AccountModel Leave(MyClientPeer client)
        {
            if (!room.clientAccountDict.ContainsKey(client))
                return null;

            AccountModel model = room.clientAccountDict[client];
            room.clientAccountDict.Remove(client);
            return model;
        }

        //获取房间模型
        public RoomModel GetRoomModel()
        {
            return room;
        }
    }
}
