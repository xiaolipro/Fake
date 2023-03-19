using System;
using System.Collections.Generic;
using Fake.Domain;

namespace Domain.Aggregations;

public class Address:ValueObject
{
    public Guid CityId { get; }

    public string Street { get; }

    public int Number { get; }
    
    public Address(Guid cityId, string street, int number)
    {
        CityId = cityId;
        Street = street;
        Number = number;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CityId;
        yield return Street;
        yield return Number;
    }
}