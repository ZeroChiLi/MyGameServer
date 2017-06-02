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

        public bool Enter(MyClientPeer client,AccountModel model)
        {
            if (room.Contains(client))
                return false;

            room.Add(client,model);
            return true;
        }
    }
}
