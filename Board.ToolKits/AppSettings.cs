using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.ToolKits
{
    /// <summary>
    /// appsettings.json配置文件数据读取类
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 配置文件的根节点
        /// </summary>
        private static readonly IConfigurationRoot _config;
        public static IConfigurationRoot Config;

        /// <summary>
        /// Constructor
        /// </summary>
        static AppSettings()
        {
            // 加载appsettings.json，并构建IConfigurationRoot
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true, true);
            _config = Config = builder.Build();

        }

        public static string app(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return _config[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return string.Empty;
        }

        /// <summary>
        /// ApiVersion
        /// </summary>
        public static string? ApiVersion => _config["ApiVersion"];

        /// <summary>
        /// ListenPort
        /// </summary>
        public static string? ListenPort => _config["ListenPort"];

        /// <summary>
        /// WareHouseId
        /// </summary>
        public static string? WareHouseId => _config["WareHouseId"];

        /// <summary>
        /// EnableDb
        /// </summary>
        public static string? EnableDb => _config["ConnectionStrings:Enable"];

        /// <summary>
        /// ConnectionStrings
        /// </summary>
        public static string? ConnectionStrings => _config.GetConnectionString(EnableDb);

    }
}
