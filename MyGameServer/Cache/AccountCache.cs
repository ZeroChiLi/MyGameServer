using MyGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGameServer.Cache
{
    public class AccountCache
    {
        #region 注册
        //帐号ID、模型的映射
        private Dictionary<int, AccountModel> idModelDict;
        private int index = 0;

        //添加帐号
        public void Add(string acc,string pwd)
        {
            idModelDict.Add(index, new AccountModel(index, acc, pwd));
            index++;
        }

        //是否已经含有该账户
        public bool HasAccount(string account)
        {
            foreach(AccountModel model in idModelDict.Values)
            {
                if (account == model.Account)
                    return true;
            }
            return false;
        }

        //检测帐号密码是否匹配
        public bool IsMatch(string account,string password)
        {
            foreach(AccountModel model in idModelDict.Values)
            {
                if (model.Account == account && model.Password == password)
                    return true;
            }
            return false;

        }

        #endregion

        #region 登录

        //在线玩家客户端和帐号模型映射
        private Dictionary<MyClientPeer, AccountModel> clientModelDict;

        //用户是否在线
        public bool IsOnline(MyClientPeer client)
        {
            return clientModelDict.ContainsKey(client);
        }

        //玩家上线
        public void Online(MyClientPeer client,string account, string password)
        {
            foreach (AccountModel model in idModelDict.Values)
            {
                if (model.Account == account && model.Password == password)
                    clientModelDict.Add(client, model);
            }
        }

        //玩家下线
        public void Offline(MyClientPeer client)
        {
            clientModelDict.Remove(client);
        }

        public AccountModel GetModel(MyClientPeer client)
        {
            AccountModel model = null;
            clientModelDict.TryGetValue(client, out model);

            return model;
        }

        #endregion

        public AccountCache()
        {
            idModelDict = new Dictionary<int, AccountModel>();
            clientModelDict = new Dictionary<MyClientPeer, AccountModel>();
        }
    }
}
