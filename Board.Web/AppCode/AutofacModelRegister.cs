using Autofac;
using AutoMapper;
using Board.Application;
using Board.Infrastructure;
using System.Reflection;

namespace Board.Web.AppCode
{
    public class AutofacModelRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //base.Load(builder);
            // 单个服务类注册
            //builder.RegisterType<TestService>().As<ITestService>().InstancePerLifetimeScope();

            builder.RegisterType<Mapper>().As<IMapper>().SingleInstance();//.InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsImplementedInterfaces()
                                                                .InstancePerLifetimeScope()
                                                                .PropertiesAutowired();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance();
            //IEntity
            //IBaseDomain
            //IBaseService

            var domain = Assembly.Load("Board.Domain");
            var service = Assembly.Load("Board.Application");
            builder.RegisterAssemblyTypes(domain)
                .AsImplementedInterfaces()//实现的接口
                .InstancePerLifetimeScope()
                .PropertiesAutowired();//属性注入
        }
    }
}
