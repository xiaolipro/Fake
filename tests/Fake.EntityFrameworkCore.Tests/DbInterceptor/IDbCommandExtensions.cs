using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace DbInterceptor
{
    //
    // 摘要:
    //     Internal MiniProfiler extensions, not meant for consumption. This can and probably
    //     will break without warning. Don't use the .Internal namespace directly.
    public static class IDbCommandExtensions
    {
        private static readonly Regex commandSpacing = new Regex(",([^\\s])", RegexOptions.Compiled);


        //
        // 摘要:
        //     Gets a command's text, adding space around crowded commas for readability.
        //
        // 参数:
        //   command:
        //     The command to space out.
        public static string GetReadableCommand(this IDbCommand command)
        {
            if (command == null)
            {
                return string.Empty;
            }

            return commandSpacing.Replace(command.CommandText, ", $1");
        }

        //
        // 摘要:
        //     Returns better parameter information for command. Returns null if no parameters
        //     are present.
        //
        // 参数:
        //   command:
        //     The command to get parameters for.
        public static List<SqlTimingParameter>? GetParameters(this IDbCommand command)
        {
            if ((command?.Parameters?.Count).GetValueOrDefault() == 0)
            {
                return null;
            }

            List<SqlTimingParameter> list = new List<SqlTimingParameter>();
            foreach (DbParameter parameter in command.Parameters)
            {
                if (parameter.ParameterName.HasValue())
                {
                    list.Add(new SqlTimingParameter
                    {
                        Name = parameter.ParameterName.Trim(),
                        Value = parameter.GetStringValue(),
                        DbType = parameter.DbType.ToString(),
                        Size = parameter.GetSize(),
                        Direction = parameter.Direction.ToString(),
                        IsNullable = parameter.IsNullable
                    });
                }
            }

            return list;
        }
    }
}
