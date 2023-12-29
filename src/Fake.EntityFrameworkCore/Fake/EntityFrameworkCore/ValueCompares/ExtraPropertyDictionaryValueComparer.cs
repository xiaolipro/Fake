using System;
using System.Linq;
using Fake.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fake.EntityFrameworkCore.ValueCompares;

public class ExtraPropertyDictionaryValueComparer() : ValueComparer<ExtraPropertyDictionary>(
    (d1, d2) => Compare(d1, d2),
    d => d.Aggregate(0, (k, v) => HashCode.Combine(k, v.GetHashCode())),
    d => new ExtraPropertyDictionary(d))
{
    private static bool Compare(ExtraPropertyDictionary? a, ExtraPropertyDictionary? b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a is null)
        {
            return b is null;
        }

        return b is not null && a.SequenceEqual(b);
    }
}