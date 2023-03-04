using System.Collections.Generic;

namespace Fake.Logging;

public interface ICanLog
{
    List<string> Logs { get; }
}