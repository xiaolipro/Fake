using System.Data.Common;

namespace Fake.EntityFrameworkCore.Interceptors;

public interface ICommandFormatter
{
    string Format(DbCommand command);
}