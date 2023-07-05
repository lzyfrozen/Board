using Board.ToolKits;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Board.Infrastructure.DBHelpers
{
    public class DBHelper
    {

        public static IDbConnection Connection
        {
            get
            {
                var connectionString = AppSettings.app(new string[] { "ConnectionStrings", "Oracle" });
                var conn = new OracleConnection(connectionString); ;
                return conn;
            }
        }

    }
}
