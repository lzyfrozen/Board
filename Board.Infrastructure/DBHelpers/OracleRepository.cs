using Board.Infrastructure.Models;
using Board.ToolKits;
using Dapper;
using System.Data;
using System.Data.Common;
using static Dapper.SqlMapper;

namespace Board.Infrastructure.DBHelpers
{
    public class OracleRepository : IOracleRepository
    {
        protected DbConnection conn
        {
            get
            {
                return (DbConnection)DBHelper.Connection;
            }
        }
        public GridReader QueryMultiple(string query, object? parameters = null)
        {
            return conn.QueryMultiple(query, parameters);
        }

        public IEnumerable<TAny> Query<TAny>(string query, object? parameters = null)
        {
            using (var conn = DBHelper.Connection)
            {
                return conn.Query<TAny>(query, parameters);
            }
        }

        public Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object? parameters = null)
        {
            using (var conn = DBHelper.Connection)
            {
                return conn.QueryAsync<TAny>(query, parameters);
            }
        }

        public int Execute(string query, object? parameters = null)
        {
            using (var conn = DBHelper.Connection)
            {
                return conn.Execute(query, parameters);
            }
        }

        public Task<int> ExecuteAsync(string query, object? parameters = null)
        {
            using (var conn = DBHelper.Connection)
            {
                return conn.ExecuteAsync(query, parameters);
            }
        }

        public T ExecuteScalar<T>(string query, object? parameters = null)
        {
            using (var conn = DBHelper.Connection)
            {
                return conn.ExecuteScalar<T>(query, parameters);
            }
        }

        public Task<T> ExecuteScalarAsync<T>(string query, object? parameters = null)
        {
            using (var conn = DBHelper.Connection)
            {
                return conn.ExecuteScalarAsync<T>(query, parameters);
            }
        }

        public async Task<DataTable> GetEmptyTableSchema(string tableName)
        {
            using (var conn = DBHelper.Connection)
            {
                var sql = $"select * from {tableName} where 1 = 2";
                var reader = await conn.ExecuteReaderAsync(sql);
                DataTable dt = new DataTable();
                dt.Load(reader);
                dt.TableName = tableName;
                return dt;
            }
        }

        public async Task<DataTable> GetEmptyTableSchema(string tableName, string emptySql)
        {
            using (var conn = DBHelper.Connection)
            {
                var reader = await conn.ExecuteReaderAsync(emptySql);
                DataTable dt = new DataTable();
                dt.Load(reader);
                dt.TableName = tableName;
                return dt;
            }
        }

        /// <summary>
        /// 带out存储过程
        /// </summary>
        /// <typeparam name="TAny"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<TAny> ExecuteProcedureOfOut<TAny>(string procName, OracleDynamicParameters param)
        {
            using (var conn = DBHelper.Connection)
            {
                var data = conn.Query<TAny>(procName, param, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        /// <summary>
        /// 带游标存储过程 返回集合
        /// </summary>
        /// <typeparam name="TAny"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<TAny> ExecuteProcedureOfCursor<TAny>(string procName, OracleDynamicParameters param)
        {
            using (var conn = DBHelper.Connection)
            {
                var data = conn.Query<TAny>(procName, param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public IEnumerable<TAny> GetPagedList<TAny>(string tables, string fields, string where, PagedAndSortedResultRequestInput queryInput, out int totalCount)
        {
            var dynaparams = new DynamicParameters();
            System.Reflection.PropertyInfo[] properties = queryInput.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var p in properties)
            {
                if (where.IndexOf("@" + p.Name, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    dynaparams.Add("@" + p.Name, p.GetValue(queryInput, null));
                }
            }
            queryInput.Normalize();
            return GetPagedList<TAny>(tables, fields, GetSortString(typeof(TAny), queryInput), where, queryInput.SkipCount, queryInput.MaxResultCount, out totalCount, dynaparams);

        }

        public IEnumerable<TAny> GetPagedList<TAny>(string tables, string fields, string where, PagedAndSortedResultRequestInput queryInput, Dictionary<string, dynamic> dicParams, out int totalCount)
        {
            var dynaparams = new DynamicParameters();
            foreach (var param in dicParams)
            {
                dynaparams.Add("@" + param.Key, param.Value);
            }
            System.Reflection.PropertyInfo[] properties = queryInput.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var p in properties)
            {
                if (where.IndexOf("@" + p.Name, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    if (!dynaparams.ParameterNames.Contains(p.Name))
                    {
                        dynaparams.Add("@" + p.Name, p.GetValue(queryInput, null));
                    }
                }
            }
            queryInput.Normalize();
            return GetPagedList<TAny>(tables, fields, GetSortString(typeof(TAny), queryInput), where, queryInput.SkipCount, queryInput.MaxResultCount, out totalCount, dynaparams);
        }

        public DataTable GetPagedList(string table, string condition, string order, int start, int limit)
        {
            var recordCount = -1;
            return GetPagedList(table, condition, order, start, limit, ref recordCount);
        }

        public DataTable GetPagedList(string table, string condition, string order, int start, int limit, ref int totalCount)
        {
            using (var conn = DBHelper.Connection)
            {
                string sql;
                if (totalCount != -1)
                {
                    sql = $@"select count(*) from {table} t where 1=1 {(condition.HasValue() ? "" : $" and {condition}")}";
                    totalCount = conn.ExecuteScalar<int>(sql, commandTimeout: 180);
                }
                var from = start + 1;
                var to = start + limit;
                sql = $@"select * from (select ROW_NUMBER() over(order by {order}) as RowNumber, * from {table} t 
                                        where 1=1 {(condition.HasValue() ? "" : $" and {condition}")}
                                        ) as temp 
                            where RowNumber between {from} and {to}";
                var reader = conn.ExecuteReader(sql, commandTimeout: 180);
                var dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        private IEnumerable<TAny> GetPagedList<TAny>(string tables, string fields, string sorts, string where, int start, int limit, out int totalCount, object? param = null)
        {
            using (var conn = DBHelper.Connection)
            {
                string sql;
                if (limit == 0)
                {
                    sql = $@"SELECT {fields} FROM {tables} WHERE {where} order by {sorts}";
                    var data = conn.Query<TAny>(sql, param);
                    totalCount = data.Count();
                    return data;
                }
                else
                {
                    sql = $@"select count(*) from {tables} where {where}";
                    totalCount = conn.ExecuteScalar<int>(sql, param);
                    if (start == 0)
                    {
                        sql = $@"select tt.* from 
                                (select row_number() over(order by {sorts}) as rownumber,{fields} 
                                from ({tables}) where {where}) tt 
                                where tt.rownumber<={limit}";
                    }
                    else
                    {
                        sql = $@"select tt.* from 
                                (select row_number() over(order by {sorts}) as rownumber,{fields} 
                                from ({tables}) where {where}) tt
                                where tt.rownumber between {start + 1} and {start + limit}";
                    }
                    return conn.Query<TAny>(sql, param);
                }
            }
        }

        private string GetSortString(Type listType, PagedAndSortedResultRequestInput queryInput)
        {
            var field = listType.GetProperty("SortMatch");
            if (field == null) return queryInput.Sorting;
            var value = field.GetValue(null, null);
            if (value == null) return queryInput.Sorting;
            Dictionary<string, string> sortsDic = value as Dictionary<string, string>;

            string Sorting = string.Empty;
            if (queryInput.Sort.HasValue())
            {
                var sorts = queryInput.Sort.ToObject<List<SortItem>>();
                if (sorts != null && sorts.Count > 0)
                {
                    Sorting = sorts.Select(p => getDicString(sortsDic, p.Property) + " " + p.Direction.ToUpper()).JoinAsString(",");
                }
                else
                {
                    Sorting = queryInput.Sorting;
                }
            }
            else
            {
                Sorting = $"{getDicString(sortsDic, "CreationTime")} DESC";
            }

            return Sorting;
        }

        private string getDicString(Dictionary<string, string> dic, string key)
        {
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            else
            {
                return key;
            }
        }



    }
}
