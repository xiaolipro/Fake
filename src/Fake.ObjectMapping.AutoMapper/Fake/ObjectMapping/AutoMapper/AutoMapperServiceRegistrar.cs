using System;
using System.Linq;
using AutoMapper;
using Fake.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fake.ObjectMapping.AutoMapper;

public class AutoMapperServiceRegistrar : DefaultServiceRegistrar
{
    private readonly Type[] _openTypes =
    {
        typeof(IValueResolver<,,>),
        typeof(IMemberValueResolver<,,,>),
        typeof(ITypeConverter<,>),
        typeof(IValueConverter<,>),
        typeof(IMappingAction<,>)
    };

    protected override bool IsDisableServiceRegistration(Type type)
    {
        return !type.GetInterfaces()
                   .Any(x => x.IsGenericType && _openTypes.Contains(x.GetGenericTypeDefinition())) 
               || base.IsDisableServiceRegistration(type);
    }

    protected override ServiceLifetime? GetLifeTimeOrNull(Type type, DependencyAttribute attribute)
    {
        return ServiceLifetime.Transient;
    }
}