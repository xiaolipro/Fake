using System.Globalization;

namespace System.Data;

public static class FakeDataParameterExtensions
{
    //
    // 摘要:
    //     Returns the value of parameter suitable for storage/display.
    //
    // 参数:
    //   parameter:
    //     The parameter to get a value for.
    public static string GetStringValue(this IDataParameter parameter)
    {
        object value = parameter.Value;
        if (value == null || value == DBNull.Value)
        {
            return null;
        }

        if (parameter.DbType == DbType.Binary)
        {
            byte[] array = value as byte[];
            if (array != null && array.Length <= 512)
            {
                return "0x" + BitConverter.ToString(array).Replace("-", string.Empty);
            }

            return null;
        }

        if (parameter.DbType == DbType.Date && value is DateTime)
        {
            return ((DateTime)value).ToString("d", CultureInfo.InvariantCulture);
        }

        if (parameter.DbType == DbType.Time && value is TimeSpan)
        {
            return ((TimeSpan)value).ToString("hh\\:mm\\:ss");
        }

        if (value is DateTime)
        {
            return ((DateTime)value).ToString("s", CultureInfo.InvariantCulture);
        }

        if (value is DateTimeOffset)
        {
            DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
            TimeSpan offset = dateTimeOffset.Offset;
            return dateTimeOffset.ToString("s", CultureInfo.InvariantCulture) + ((offset < TimeSpan.Zero) ? "-" : "+") +
                   offset.ToString("hh\\:mm");
        }

        Type type = value.GetType();
        if (type.IsEnum)
        {
            return Convert.ChangeType(value, Enum.GetUnderlyingType(type))!.ToString();
        }

        return Convert.ToString(value, CultureInfo.InvariantCulture);
    }

    //
    // 摘要:
    //     Gets the size of a System.Data.IDbDataParameter (e.g. nvarchar(20) would be 20).
    //
    // 参数:
    //   parameter:
    //     The parameter to get the size of.
    //
    // 返回结果:
    //     The size of the parameter, or 0 if nullable or unlimited.
    public static int GetSize(this IDbDataParameter parameter)
    {
        if (!parameter.IsNullable || parameter.Value != null)
        {
            return parameter.Size;
        }

        return 0;
    }
}