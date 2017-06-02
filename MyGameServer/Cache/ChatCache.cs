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

        public RoomModel Enter(MyClientPeer client,AccountModel model)
        {
            if (room.Contains(client))
                return null;

            room.Add(client,model);
            return room;
        }
    }
}
