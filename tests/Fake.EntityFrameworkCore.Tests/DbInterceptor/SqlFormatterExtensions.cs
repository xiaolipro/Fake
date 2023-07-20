using System.Data;

namespace DbInterceptor
{
    /// <summary>
    /// Extensions for ISqlFormatter instances
    /// </summary>
    public static class SqlFormatterExtensions
    {

        /// <summary>
        /// Format sql using the FormatSql method available on the given <see cref="ISqlFormatter"/>.
        /// </summary>
        /// <param name="sqlFormatter">The <see cref="ISqlFormatter"/> to use.</param>
        /// <param name="command">The <see cref="IDbCommand"/> being represented.</param>
        public static string GetFormattedSql(this ISqlFormatter sqlFormatter, IDbCommand command)
        {
            var commandText = command.CommandText;
            var parameters = command.GetParameters();

            return sqlFormatter.FormatSql(commandText, parameters);
        }
    }
}
