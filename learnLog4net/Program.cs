using log4net;
using log4net.Config;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace learnLog4net
{
    class Program
    {
        static void Main(string[] args)
        {
            InitLog4Net();

            //             var loggerConsole = LogManager.GetLogger("UB.Console");
            //             loggerConsole.Debug("调试");
            //             loggerConsole.Info("消息");
            // 
            //             var loggerFile = LogManager.GetLogger("UB.File");
            //             loggerFile.Debug("调试");
            //             loggerFile.Fatal("错误");
            // 
            //             var loggerDatabase = LogManager.GetLogger("UB.Database");
            //             loggerDatabase.Debug("调试");
            //             loggerDatabase.Fatal("错误");
            // 
            //             var loggerUDP = LogManager.GetLogger("UB.UDP");
            //             loggerUDP.Debug("调试");
            //             loggerUDP.Fatal("错误");

            var loggerAll = LogManager.GetLogger("UB.ALL");
            loggerAll.Debug("调试");
            loggerAll.Fatal("错误");



            Console.WriteLine("hello");

        }
        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}
