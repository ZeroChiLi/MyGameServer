using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using Code.Common;
using MyGameServer.Cache;
using Common.Dto;
using LitJson;
using 

namespace MyGameServer.Logic
{
    class AccountHandler : IHandler
    {
        AccountCache cache = Factory.AccountCache;

        //客户端下线
        public void OnDisconnect(MyClientPeer client)
        {
            cache.Offline(client);
        }

        //请求处理
        public void OnRequest(MyClientPeer client, byte subCode, OperationRequest request)
        {
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
            OperationResponse response = new OperationResponse((byte)OpCode.Account, new Dictionary<byte, object>());
            if (cache.HasAccount(account.Account))
            {
                SendResponseWithInformation(client, response, "该帐号已经存在", -1);
            }
            else
            {
                cache.Add(account.Account, account.Password);

                SendResponseWithInformation(client, response, "注册成功", 0);

            }
            return;
        }

        //登录处理
        private void Login(MyClientPeer client, AccountDto account)
        {
            OperationResponse response = new OperationResponse((byte)OpCode.Account, new Dictionary<byte, object>());
            if(!cache.HasAccount(account.Account))
            {
                SendResponseWithInformation(client, response, "该帐号不存在", -1);
                return;
            }
            else if (!cache.IsMatch(account.Account, account.Password))
            {
                SendResponseWithInformation(client, response, "帐号密码不匹配", -2);
                return;
            }
            else if (cache.IsOnline(client))
            {
                SendResponseWithInformation(client, response, "玩家已经在线", -3);
                return;
            }
            else
            {
                cache.Online(client,account.Account,account.Password);
                SendResponseWithInformation(client, response, "登陆成功", 1);
            }
        }

        private void SendResponseWithInformation(MyClientPeer client,OperationResponse response,string debugMessage = "", short returnCode = 0)
        {
            response.DebugMessage = debugMessage;
            response.ReturnCode = returnCode;
            client.SendOperationResponse(response, new SendParameters());
        }

    }
}
