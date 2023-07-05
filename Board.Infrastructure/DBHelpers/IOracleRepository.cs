using Board.Infrastructure.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Board.Infrastructure.DBHelpers
{
    public interface IOracleRepository
    {
        /*
        /// <summary>
        /// 获取当前仓库的一个单证流水号
        /// </summary>
        /// <param name="snRuleTypeCode"></param>
        /// <returns></returns>
        string GetNextNo(string snRuleTypeCode);
        /// <summary>
        /// 获取一个单证流水号
        /// </summary>
        /// <param name="snRuleTypeCode"></param>
        /// <param name="whId">仓库Id</param>
        /// <returns></returns>
        string GetNextNo(string snRuleTypeCode, string whId);

        /// <summary>
        /// 获取多个流水号
        /// </summary>
        /// <param name="snRuleTypeCode"></param>
        /// <param name="whId">仓库Id</param>
        /// <param name="snCount">流水号数量</param>
        /// <returns></returns>
        List<string> GetNextNo(string snRuleTypeCode, string whId, int snCount);
        */
        GridReader QueryMultiple(string query, object? parameters = null);
        IEnumerable<TAny> Query<TAny>(string query, object? parameters = null);
        Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object? parameters = null);
        int Execute(string query, object? parameters = null);
        Task<int> ExecuteAsync(string query, object? parameters = null);
        T ExecuteScalar<T>(string query, object? parameters = null);
        Task<T> ExecuteScalarAsync<T>(string query, object? parameters = null);
        Task<DataTable> GetEmptyTableSchema(string tableName);
        Task<DataTable> GetEmptyTableSchema(string tableName, string emptySql);

        /// <summary>
        /// 带out存储过程
        /// </summary>
        /// <typeparam name="TAny"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TAny> ExecuteProcedureOfOut<TAny>(string procName, OracleDynamicParameters param);

        /// <summary>
        /// 带游标存储过程 返回集合
        /// </summary>
        /// <typeparam name="TAny"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TAny> ExecuteProcedureOfCursor<TAny>(string procName, OracleDynamicParameters param);

        IEnumerable<TAny> GetPagedList<TAny>(string tables, string fields, string where, PagedAndSortedResultRequestInput queryInput, out int totalCount);
        IEnumerable<TAny> GetPagedList<TAny>(string tables, string fields, string where, PagedAndSortedResultRequestInput queryInput, Dictionary<string, dynamic> dicParams, out int totalCount);

        DataTable GetPagedList(string table, string condition, string order, int start, int limit);

        DataTable GetPagedList(string table, string condition, string order, int start, int limit, ref int totalCount);


        /*
        string CurrentWarehouseId { get; }
        
        DataTable GetEmptyDataTable(string sql);

        void BulkInsert(DataTable dt);
        

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="TEntity">EF的entity</typeparam>
        /// <param name="entities">Entity列表</param>
        void BulkInsert<TEntity>(IList<TEntity> entities);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="TEntity">EF的entity</typeparam>
        /// <param name="entities">Entity列表</param>
        /// <param name="batchSize">每个批处理中的行数。如果未设置任何值，则为零。</param>
        /// <param name="bulkCopyTimeout">超时之前可用于完成操作的秒数。 默认值为 30 秒。 值为 0 表示无限制，大容量复制将无限期等待。</param>
        void BulkInsert<TEntity>(IList<TEntity> entities, int? batchSize, int? bulkCopyTimeout);
        */
    }
}
