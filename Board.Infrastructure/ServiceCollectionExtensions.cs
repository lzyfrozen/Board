using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AutoMapper;
using Board.Infrastructure.DBHelpers;
using Board.Infrastructure.HttpClientFactory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Board.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoInject(this IServiceCollection services)
        {
            var allTypes = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(p => !Path.GetFileName(p).StartsWith("System.")
                            && !Path.GetFileName(p).StartsWith("Microsoft."))
                .Select(Assembly.LoadFrom)
                .SelectMany(y => y.DefinedTypes)
                .ToList();
            allTypes?.ForEach(thisType =>
            {
                //注入的限制
                var allInterfaces = thisType.GetInterfaces().Where(p => p.GetInterfaces().Contains(typeof(IBaseService)) || p.GetInterfaces().Contains(typeof(IBaseDomain))).ToList();
                allInterfaces?.ForEach(thisInterface => {
                    services.AddScoped(thisInterface, thisType);//只要符合条件的所有接口都注入t类
                });
            });

            services.AddScoped<IOracleRepository, OracleRepository>();
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEdiRequest, EdiRequest>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //根据属性注入来配置全局拦截器
            services.ConfigureDynamicProxy(config =>
            {
                config.Interceptors.AddTyped<TransactionalAttribute>();//TransactionalAttribute 这个是需要全局拦截的拦截器
            });
        }

        /// <summary>
        /// 自动创建映射
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var allProfile = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Where(p => !Path.GetFileName(p).StartsWith("System.")
                            && !Path.GetFileName(p).StartsWith("Microsoft."))
                .Select(Assembly.LoadFrom)
                .SelectMany(y => y.DefinedTypes)
                .Where(x => typeof(Profile).IsAssignableFrom(x) && x.IsClass)?
                .ToArray();
            if (allProfile != null)
                services.AddAutoMapper(allProfile);


            #region Program.cs

            //var configuration = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<LocationProfile>();
            //    cfg.AddProfile<InTaskProfile>();
            //    cfg.AddProfile<OutTaskProfile>();
            //    cfg.AddProfile<FGOutTaskProfile>();
            //    cfg.AddProfile<FGInPlanProfile>();
            //    cfg.AddProfile<InOutQtyProfile>();
            //});
            //builder.Services.AddSingleton(configuration); 

            #endregion
        }
    }
}
