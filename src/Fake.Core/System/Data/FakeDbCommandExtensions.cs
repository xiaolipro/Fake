using System.Data.Common;
using System.Text.RegularExpressions;
using Fake.Data;

namespace System.Data;

public static class FakeDbCommandExtensions
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
    [CanBeNull]
    public static List<SqlTimingParameter> GetParameters(this IDbCommand command)
    {
        if ((command?.Parameters?.Count).GetValueOrDefault() == 0)
        {
            return null;
        }

        List<SqlTimingParameter> list = new List<SqlTimingParameter>();
        foreach (DbParameter parameter in command.Parameters)
        {
            if (!parameter.ParameterName.IsNullOrEmpty())
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