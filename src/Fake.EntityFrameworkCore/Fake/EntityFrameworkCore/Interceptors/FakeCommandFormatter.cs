using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace Fake.EntityFrameworkCore.Interceptors;

public class FakeCommandFormatter(IFakeClock clock) : ICommandFormatter
{
    public string Format(DbCommand command)
    {
        if (command.Parameters.Count == 0)
        {
            return command.CommandText;
        }

        var formattedCommand = new StringBuilder(command.CommandText);
        foreach (DbParameter parameter in command.Parameters)
        {
            var parameterName = parameter.ParameterName;
            var parameterValue = GetParameterValue(parameter);
            var parameterValueWrapper = GetParameterValueWrapper(parameter, parameterValue);
            formattedCommand.Replace(parameterName, parameterValueWrapper);
        }

        return formattedCommand.ToString();
    }

    private string? GetParameterValueWrapper(IDataParameter parameter, string? parameterValue)
    {
        if (parameter.Value is string or char or Guid or DateTime) return $"'{parameterValue}'";
        return parameterValue;
    }

    public virtual string? GetParameterValue(IDataParameter parameter)
    {
        var value = parameter.Value;
        if (value == null || value == DBNull.Value)
        {
            return null;
        }

        if (value is byte[] bytes)
        {
            return "0x" + BitConverter.ToString(bytes);
        }

        if (value is DateTime datetime)
        {
            return clock.NormalizeAsString(datetime);
        }

        // todo: bool在postgresql表达是0,1

        if (value.GetType().IsEnum)
        {
            return Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType())).ToString();
        }

        return Convert.ToString(value, CultureInfo.InvariantCulture);
    }
}