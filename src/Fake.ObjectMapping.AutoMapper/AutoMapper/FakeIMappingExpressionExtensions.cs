using System;
using System.Linq.Expressions;

namespace AutoMapper;

public static class FakeIMappingExpressionExtensions
{
    public static IMappingExpression<TDestination, TMember> Ignore<TDestination, TMember, TResult>(this IMappingExpression<TDestination, TMember> mappingExpression, Expression<Func<TMember, TResult>> destinationMember)
    {
        return mappingExpression.ForMember(destinationMember, opts => opts.Ignore());
    }
}