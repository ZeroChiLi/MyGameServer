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
        //日志文件 在Photon目录下deploy/log文件夹下
        private static readonly ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();

        //服务器启动时
        protected override void Setup()
        {
            InitLogging();

            Log("调用 Setup()!");
        }

        //创建连接，每个客户端的入口
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new MyClientPeer(initRequest);
        }

        //初始化日志
        protected virtual void InitLogging()
        {
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "-666-" + this.ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
        }

        //服务器关闭时
        protected override void TearDown()
        {
            Log("调用 TearDown!");
        }

        //输出日志到日志文件里
        public static void Log(string message)
        {
            log.Info(message.ToString());
        }

    }
}
