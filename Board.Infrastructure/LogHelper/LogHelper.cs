using Serilog.Events;
using Serilog;
using System.Text;
using System.Diagnostics;
using Board.ToolKits;

namespace Board.Infrastructure
{
    public class LogHelper
    {
        static LogHelper()
        {

        }

        public static void SerilogSetting()
        {
            string LogFilePath(string LogEvent) => $@"{Directory.GetCurrentDirectory()}/Logs/{DateTime.Now:yyyy-MM-dd}/{LogEvent}/log-.log";
            string SerilogOutputTemplate = @"{NewLine}时间:{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}等级:{Level:u3}{NewLine}来源:{SourceContext}{NewLine}具体消息:{Message:lj}{NewLine}{Exception}" + new string('-', 50);
            //string SerilogOutputTemplate = @"{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level:u3}{NewLine}Source:{SourceContext}{NewLine}Message：{Message:lj}{NewLine}{Exception}" + new string('-', 50);
            //    @"{Timestamp:yyyy-MM-dd HH:mm-ss.fff }[{Level:u3}] {Message:lj}{NewLine}{Exception}",
            // 创建全局静态实例
            Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Debug()
                //.MinimumLevel.Override("System", LogEventLevel.Warning)
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                //.WriteTo.Console()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(AppSettings.Config)
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug)
                                    .WriteTo.File(LogFilePath("Debug"),
                                        outputTemplate: SerilogOutputTemplate,
                                        rollingInterval: RollingInterval.Day,
                                        rollOnFileSizeLimit: true,
                                        encoding: Encoding.UTF8,
                                        retainedFileCountLimit: 100,
                                        fileSizeLimitBytes: 10 * 1024 * 1024))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information)
                                    .WriteTo.File(LogFilePath("Information"),
                                        outputTemplate: SerilogOutputTemplate,
                                        rollingInterval: RollingInterval.Day,
                                        rollOnFileSizeLimit: true,
                                        encoding: Encoding.UTF8,
                                        retainedFileCountLimit: 100,
                                        fileSizeLimitBytes: 10 * 1024 * 1024))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning)
                                    .WriteTo.File(LogFilePath("Warning"),
                                        outputTemplate: SerilogOutputTemplate,
                                        rollingInterval: RollingInterval.Day,
                                        rollOnFileSizeLimit: true,
                                        encoding: Encoding.UTF8,
                                        retainedFileCountLimit: 100,
                                        fileSizeLimitBytes: 10 * 1024 * 1024))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error)
                                    .WriteTo.File(LogFilePath("Error"),
                                        outputTemplate: SerilogOutputTemplate,
                                        rollingInterval: RollingInterval.Day,
                                        rollOnFileSizeLimit: true,
                                        encoding: Encoding.UTF8,
                                        retainedFileCountLimit: 100,
                                        fileSizeLimitBytes: 10 * 1024 * 1024))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal)
                                    .WriteTo.File(LogFilePath("Fatal"),
                                        outputTemplate: SerilogOutputTemplate,
                                        rollingInterval: RollingInterval.Day,
                                        rollOnFileSizeLimit: true,
                                        encoding: Encoding.UTF8,
                                        retainedFileCountLimit: 100,
                                        fileSizeLimitBytes: 10 * 1024 * 1024))
                //.WriteTo.MSSqlServer(connecting, "logs", autoCreateSqlTable: true, restrictedToMinimumLevel: LogEventLevel.Information)
                //.WriteTo.SQLite($"{Path.Combine(Directory.GetCurrentDirectory(), "db/log.db")}", restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();
        }

        public void SerilogTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Log.Verbose("Hello SeriLog！");
            Log.Information("开始测试");

            for (var i = 0; i < 3; i++)  // 10000000000
            {
                try
                {
                    Log.Debug("抛出异常");
                    throw new InvalidProgramException("程序错误。");
                }
                catch (Exception e)
                {
                    Log.Error(e, "捕获异常");
                }
            }
            stopWatch.Stop();
            Log.Information($"结束测试， 共运行{stopWatch.ElapsedMilliseconds}ms。");
            //Log.CloseAndFlush();

            Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }
    }
}
