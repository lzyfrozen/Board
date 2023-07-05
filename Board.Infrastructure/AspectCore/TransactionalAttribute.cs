using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Infrastructure
{
    /// <summary>
    /// 为工作单元提供事务一致性
    /// </summary>
    public class TransactionalAttribute : AbstractInterceptorAttribute
    {
        IUnitOfWork? _unitOfWork { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                _unitOfWork = context.ServiceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
                _unitOfWork?.BeginTransaction();
                Console.WriteLine("-----Aspect执行前-----");
                await next(context);
                _unitOfWork?.Commit();
            }
            catch (Exception)
            {
                Console.WriteLine("-----Aspect执行异常-----");
                _unitOfWork?.Rollback();
                throw;
            }
            finally
            {
                Console.WriteLine("-----Aspect执行后-----");
            }
        }
    }
}
