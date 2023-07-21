using System.Data;
using System.Runtime.Serialization;

namespace Fake.EntityFrameworkCore.Interceptors;

//
// 摘要:
//     Information about a DbParameter used in the sql statement profiled by SqlTiming.
[DataContract]
public class CommandParameter
{
    //
    // 摘要:
    //     Holds the maximum size that will be stored for byte[] parameters
    internal const int MaxByteParameterSize = 512;

    //
    // 摘要:
    //     Parameter name, e.g. "routeName"
    [DataMember(Order = 1)] public string Name { get; set; } = "<unknown>";


    //
    // 摘要:
    //     The value submitted to the database.
    [DataMember(Order = 2)] public string? Value { get; set; }

    //
    // 摘要:
    //     System.Data.DbType, e.g. "String", "Bit"
    [DataMember(Order = 3)] public DbType DbType { get; set; }

    //
    // 摘要:
    //     How large the type is, e.g. for string, size could be 4000
    [DataMember(Order = 4)] public int Size { get; set; }

    //
    // 摘要:
    //     System.Data.ParameterDirection: "Input", "Output", "InputOutput", "ReturnValue"
    [DataMember(Order = 5)] public string? Direction { get; set; }

    //
    // 摘要:
    //     Gets or sets a value that indicates whether the parameter accepts null values.
    [DataMember(Order = 6)] public bool IsNullable { get; set; }

    //
    // 摘要:
    //     Returns true if this has the same parent StackExchange.Profiling.SqlTimingParameter.Name
    //     and StackExchange.Profiling.SqlTimingParameter.Value as obj.
    //
    // 参数:
    //   obj:
    //     The System.Object to compare.
    public override bool Equals(object obj)
    {
        CommandParameter commandParameter = obj as CommandParameter;
        if (commandParameter != null && string.Equals(Name, commandParameter.Name))
        {
            return string.Equals(Value, commandParameter.Value);
        }

        return false;
    }

    //
    // 摘要:
    //     Returns the XOR of certain properties.
    public override int GetHashCode()
    {
        int num = Name.GetHashCode();
        if (Value != null)
        {
            num ^= Value!.GetHashCode();
        }

        return num;
    }

    //
    // 摘要:
    //     Returns name and value for debugging.
    public override string ToString()
    {
        return Name + " = " + Value + " (" + DbType + ")";
    }
}