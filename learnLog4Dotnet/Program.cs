using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace learnLog4Dotnet
{
    class Program
    {
        public static ILog DefaultLogger;

        public static ILog HardwareLogger;
        static void Main(string[] args)
        {
            InitLog4Net();

            var loggerConsole = LogManager.GetLogger("UB.Console");
            loggerConsole.Debug("调试");
            loggerConsole.Info("消息");

            var loggerFile = LogManager.GetLogger("UB.File");
            loggerFile.Debug("调试");
            loggerFile.Fatal("错误");

            Console.WriteLine("hello");

        }
        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}
