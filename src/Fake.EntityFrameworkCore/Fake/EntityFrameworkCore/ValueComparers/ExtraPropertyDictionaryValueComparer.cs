using System;
using System.Linq;
using Fake.Data;
using Fake.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fake.EntityFrameworkCore.ValueComparers;

public class ExtraPropertyDictionaryValueComparer : ValueComparer<ExtraPropertyDictionary>
{
    public ExtraPropertyDictionaryValueComparer()
        : base(
            (d1, d2) => d1.SequenceEqual(d2),
            d => d.Aggregate(0, (k, v) => HashCode.Combine(k, v.GetHashCode())),
            d => new ExtraPropertyDictionary(d))
    {
    }
}
