using System.Collections.Generic;
using Photon.SocketServer;
using MyGameServer.Cache;
using Common.Dto;
using LitJson;
using Common.Code;

namespace MyGameServer.Logic
{
    //用户请求处理
    class AccountHandler : Handler
    {
        AccountsCache accountCache = Factory.accountCache;

        //服务器回复给客户端的信息
        OperationResponse response = new OperationResponse((byte)OpCode.Account, new Dictionary<byte, object>());


        //客户端下线
        public override void OnDisconnect(MyClientPeer client)
        {
            accountCache.OffLine(client);
        }

        //处理客户端请求处理
        public override void OnRequest(MyClientPeer client, byte subCode, OperationRequest request)
        {
            //获取发送请求的用户模版信息
            AccountDto dto = JsonMapper.ToObject<AccountDto>(request.Parameters[0].ToString()) as AccountDto;
            switch ((AccountCode)subCode)
            {
                case AccountCode.Login:
                    Login(client, dto);
                    break;
                case AccountCode.Register:
                    Register(client, dto);
                    break;
            }

        }

        //注册处理
        private void Register(MyClientPeer client, AccountDto account)
        {
            response.Parameters[80] = AccountCode.Register;
            if (accountCache.Contain(account.AccountName))
            {
                SendResponseToClient(client, response, "该帐号已经存在", -1);
            }
            else
            {
                accountCache.Add(account.AccountName, account.Password);
                SendResponseToClient(client, response, "注册成功", 0);
            }
            return;
        }

        //登录处理
        private void Login(MyClientPeer client, AccountDto account)
        {
            response.Parameters[80] = AccountCode.Login;
            if(!accountCache.Contain(account.AccountName))
            {
                SendResponseToClient(client, response, "该帐号不存在", -1);
            }
            else if (!accountCache.IsMatch(account.AccountName, account.Password))
            {
                SendResponseToClient(client, response, "帐号密码不匹配", -2);
            }
            else if (accountCache.IsOnline(account.AccountName))
            {
                SendResponseToClient(client, response, "玩家已经在线", -3);
            }
            else
            {
                accountCache.OnLine(client,account.AccountName,account.Password);
                SendResponseToClient(client, response, "登陆成功", 0);
            }
        }



    }
}
