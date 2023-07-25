﻿using Fake.Logging;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class FakeServiceCollectionLoggingExtensions
{
    public static ILogger<T> GetInitLogger<T>(this IServiceCollection services)
    {
        return services.GetInstance<IInitLoggerFactory>().Create<T>();
    }
}