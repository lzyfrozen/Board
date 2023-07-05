using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Board.Infrastructure
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private Guid _id = Guid.Empty;
        private bool _disposed = false;
        private IDbConnection? _connection = null;
        private IDbTransaction? _trans = null;

        /// <summary>
        /// 数据连接
        /// </summary>
        public IDbConnection? DbConnection { get { return _connection; } }

        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction? DbTransaction { get { return _trans; } }

        public UnitOfWork(IConfiguration configuration)
        {
            _id = new Guid();
            _connection = new OracleConnection(configuration.GetConnectionString("ConnectionStrings:Oracle"));
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                _connection?.Open();
            }
            _trans = _connection?.BeginTransaction();
        }
        /// <summary>
        /// 完成事务
        /// </summary>
        public void Commit()
        {
            _trans?.Commit();
            Dispose();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            _trans?.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _trans?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
            }

            _trans = null;
            _connection = null;
            _disposed = true;
        }


    }
}
