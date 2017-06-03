using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGameServer.Cache
{
    public class Factory
    {
        public static AccountCache accountCache = null;
        public static ChatCache chatCache = null;

        static Factory()
        {
            accountCache = new AccountCache();
            chatCache = new ChatCache();
        }
    }
}
