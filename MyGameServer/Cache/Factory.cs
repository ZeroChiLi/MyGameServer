using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGameServer.Cache
{
    public class Factory
    {
        private static AccountCache accountCache = null;

        public static AccountCache AccountCache
        {
            get
            {
                if (accountCache == null)
                    accountCache = new AccountCache();
                return accountCache;
            }
        }
    }
}
