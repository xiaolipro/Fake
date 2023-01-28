using System.Collections.Generic;
using System.Reflection;
using Autofac.Core;
using Fake.DependencyInjection;

namespace Fake.Autofac;

public class FakePropertySelector:DefaultPropertySelector
{
    public FakePropertySelector(bool preserveSetValues) : base(preserveSetValues)
    {
    }

    public override bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        if (!propertyInfo.GetCustomAttributes(typeof(DisablePropertyInjectionAttribute), true)
                .IsNullOrEmpty()) return false;

        return base.InjectProperty(propertyInfo, instance);
    }
}