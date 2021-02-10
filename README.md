# learnLog4net

学习log4net的使用

## 配置
log4net通过xml格式的配置文件配置appender，程序启动时通过配置文件进行初始化。初始化完成后就可以通过`LogManager.GetLogger("appender_name")`获取日志接口，该日志接口即可进行日志记录。
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
    </configSections>

    <log4net>
        <root>
            <level value="ALL"/>
        </root>
        <logger name="Log4File">
            <appender-ref ref="RollingLogFileAppender"/>
        </logger>

        <logger name="Log4Console">
            <appender-ref ref="ColoredConsoleAppender"/>
        </logger>

        <logger name="Log4Database">
            <appender-ref ref="MySQLAppender"/>
        </logger>

        <logger name="Log4UDP">
            <appender-ref ref="UdpAppender"/>
        </logger>
        <logger name="Log4ALL">
            <appender-ref ref="RollingLogFileAppender"/>
            <appender-ref ref="ColoredConsoleAppender"/>
            <appender-ref ref="MySQLAppender"/>
            <appender-ref ref="UdpAppender"/>
        </logger>

        <appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
            <!--文件编码-->
            <Encoding value="utf-8" />
            <!--日志路径-->
            <File value= ".\log\"/>
            <!--是否是向文件中追加日志-->
            <AppendToFile value= "true"/>
            <!--log保留天数-->
            <MaxSizeRollBackups value= "10"/>
            <!--日志文件名是否是固定不变的-->
            <StaticLogFileName value= "false"/>
            <!--日志文件名格式为:2008-08-31.log-->
            <DatePattern value= "yyyy-MM-dd&quot;.log&quot;"/>
            <!--日志根据日期滚动-->
            <RollingStyle value= "Date"/>
            <layout type="log4net.Layout.PatternLayout">
                <ConversionPattern value="[%date{HH:mm:ss,fff}][%t][%-5level][%F:%L] %m%n" />
            </layout>
        </appender>

        <!-- 控制台前台显示日志 -->
        <appender name="ColoredConsoleAppender" type="log4net.Appender.ColoredConsoleAppender">
            <mapping>
                <level value="ERROR" />
                <foreColor value="Red, HighIntensity" />
            </mapping>
            <mapping>
                <level value="Info" />
                <foreColor value="Green" />
            </mapping>
            <layout type="log4net.Layout.PatternLayout">
                <conversionPattern value="[%date{HH:mm:ss,fff}][%-5level] %m%n" />
            </layout>

            <filter type="log4net.Filter.LevelRangeFilter">
                <LevelMin value="Info" />
                <LevelMax value="Fatal" />
            </filter>
        </appender>

        <!-- MYSQL日志 -->
        <!--DROP TABLE IF EXISTS `loggerbackstage`;
            CREATE TABLE `loggerbackstage`  (
              `id` int(11) NOT NULL AUTO_INCREMENT,
              `log_datetime` timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0),
              `log_thread` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
              `log_level` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
              `log_logger` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
              `log_message` text CHARACTER SET utf8 COLLATE utf8_general_ci,
              PRIMARY KEY (`id`) USING BTREE
            ) ENGINE = InnoDB AUTO_INCREMENT = 16 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;-->
        <appender name="MySQLAppender" type="log4net.Appender.AdoNetAppender">
            <bufferSize value="1" />

            <ConnectionType value="MySql.Data.MySqlClient.MySqlConnection, MySql.Data"/>
            <ConnectionString value="server=192.168.0.228;user id=root;password=111111;database=log4net;Charset=utf8;SSL Mode=None;"/>

            <commandText value="INSERT INTO loggerbackstage (`log_thread`,`log_level`,`log_logger`,`log_message`) VALUES (@log_thread,@log_level,@log_logger,@log_message)" />
            <parameter>
                <parameterName value="@log_thread"/>
                <dbType value="String"/>
                <size value="255"/>
                <layout type="log4net.Layout.PatternLayout">
                    <conversionPattern value="%thread"/>
                </layout>
            </parameter>
            <parameter>
                <parameterName value="@log_level"/>
                <dbType value="String"/>
                <size value="50"/>
                <layout type="log4net.Layout.PatternLayout">
                    <conversionPattern value="%-5level"/>
                </layout>
            </parameter>
            <parameter>
                <parameterName value="@log_logger"/>
                <dbType value="String"/>
                <size value="50"/>
                <layout type="log4net.Layout.PatternLayout">
                    <conversionPattern value="%logger"/>
                </layout>
            </parameter>
            <parameter>
                <parameterName value="@log_message"/>
                <dbType value="String"/>
                <size value="4000"/>
                <layout type="log4net.Layout.PatternLayout">
                    <conversionPattern value="%message"/>
                </layout>
            </parameter>
        </appender>

        <appender name="UdpAppender" type="log4net.Appender.UdpAppender">
            <!--localPort value="7170" /-->
            <remoteAddress value="192.168.0.94" />
            <remotePort value="8899" />
            <layout type="log4net.Layout.PatternLayout, log4net">
                <conversionPattern value="[%date{HH:mm:ss,fff}][%-5level] %m%n" />
            </layout>
        </appender>

    </log4net>
</configuration>
```

## 使用
通过以下代码即可获得需要的日志接口
```
    var loggerConsole = LogManager.GetLogger("Log4Console");
    loggerConsole.Debug("调试");
    loggerConsole.Info("消息");

    var loggerFile = LogManager.GetLogger("Log4File");
    loggerFile.Debug("调试");
    loggerFile.Fatal("错误");

    var loggerDatabase = LogManager.GetLogger("Log4Database");
    loggerDatabase.Debug("调试");
    loggerDatabase.Fatal("错误");

    var loggerUDP = LogManager.GetLogger("Log4UDP");
    loggerUDP.Debug("调试");
    loggerUDP.Fatal("错误");

    var loggerAll = LogManager.GetLogger("Log4ALL");
    loggerAll.Debug("调试");
    loggerAll.Fatal("错误");
```
## 注意
1. 使用数据库Appender时需要对应的dll，否则不会写入日志
2. 连接MySQL数据库时使用了选项`SSL Mode=None`是为了方便Wireshark抓包分析
3. 建表DDL已经在MySQLAppender上面给出

