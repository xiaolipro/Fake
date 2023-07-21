using System.Collections.Generic;
using Fake.Data;

namespace Fake.EntityFrameworkCore.Interceptors;

public interface ICommandFormatter
{
    /// <summary>
    /// Return SQL the way you want it to look on the in the trace. Usually used to format parameters.
    /// </summary>
    /// <param name="commandText">The SQL command to format.</param>
    /// <param name="parameters">The parameters for the SQL command.</param>
    string Format(string commandText, List<SqlTimingParameter> parameters);
}