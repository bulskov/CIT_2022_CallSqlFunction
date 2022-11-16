using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CallSqlFunction
{
    public static class DbExtensions
    {

        public static object ExecuteScalar(this NorthwindContex ctx, string sql)
        {
            var conn = (NpgsqlConnection)ctx.Database.GetDbConnection();
            conn.Open();

            var cmd = new NpgsqlCommand(sql, conn);
            return cmd.ExecuteScalar();
        }
    }
}
