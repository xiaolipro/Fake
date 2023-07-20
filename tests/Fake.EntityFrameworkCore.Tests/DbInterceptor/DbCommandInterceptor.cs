using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DbInterceptor
{
    /// <summary>
    /// DB命令Interceptor 
    /// </summary>
    public class DbCommandInterceptor : IDbCommandInterceptor
    {
        private readonly static ISqlFormatter fomatter = new InlineFormatter();

        public ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            var sql1 = fomatter.GetFormattedSql(command);
            Console.WriteLine("------------ReaderExecuting----BEGIN-------------");
            Console.WriteLine(sql1);
            Console.WriteLine("------------ReaderExecuting----END-------------");

            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }


        public ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {

            var sql1 = fomatter.GetFormattedSql(command);
            Console.WriteLine("------------NonQueryExecuting----BEGIN-------------");
            Console.WriteLine(sql1);
            Console.WriteLine("------------ NonQueryExecuting----END-------------");

            return new ValueTask<InterceptionResult<int>>(result);
        }


        public ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default(CancellationToken))
        {

            var sql1 = fomatter.GetFormattedSql(command);
            Console.WriteLine("------------ScalarExecuting----BEGIN-------------");
            Console.WriteLine(sql1);
            Console.WriteLine("------------ ScalarExecuting----END-------------");


            return new ValueTask<InterceptionResult<object>>(result);
        }


    }
}
