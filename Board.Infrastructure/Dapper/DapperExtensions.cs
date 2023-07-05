using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Infrastructure.Dapper
{
    public static class SqlMapperExtensions
    {
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="param">参数</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static PagedResult<T> GetPageList<T>(this IDbConnection connection, string tableSql, string fields, int pageIndex, int pageSize, string? condition, string? orderBy, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 20;
            var startRow = (pageIndex - 1) * pageSize;

            //MySql分页
            //sql = $"SELECT * FROM ({tableSql}) tt1  LIMIT {startRow},{pageSize};  SELECT COUNT(1) FROM ({tableSql}) tt2;";

            //Oracle分页
            var sql = $@"select tt.* from 
                        (select row_number() over(order by {orderBy}) as rownumber,{fields} 
                        from ({tableSql}) where {condition}) tt
                        where tt.rownumber between {startRow + 1} and {pageSize * pageIndex};SELECT COUNT(1) FROM ({tableSql}) tt2;";

            PagedResult<T> pagingResult = new PagedResult<T>();
            pagingResult.Paged.PageIndex = pageIndex;
            pagingResult.Paged.PageSize = pageSize;
            using (var result = connection.QueryMultiple(sql, param: param, transaction, commandTimeout))
            {
                var list = result.Read<T>();
                var totalCount = result.Read<long>().FirstOrDefault();
                pagingResult.Data = list;
                pagingResult.Paged.TotalRow = totalCount;
            }
            return pagingResult;
        }
    }
}
