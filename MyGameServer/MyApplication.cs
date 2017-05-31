using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using System.IO;
using log4net.Config;

namespace MyGameServer
{
    public class MyApplication : ApplicationBase
    {

        private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();

        public static void Log(string message)
        {
            log.Info(message.ToString());
        }

        //创建连接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new MyClientPeer(initRequest);
        }

        //服务器启动时
        protected override void Setup()
        {
            InitLogging();

            Log("It Fucking Setup!");
        }

        protected virtual void InitLogging()
        {
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "-666-" + this.ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
        }

        protected override void TearDown()
        {
            Log("It Fucking TearDown!");
        }
    }
}
