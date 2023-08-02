using System;

namespace Fake.ObjectMapping.Models.Entities;

public class TestEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime CreateTime { get; set; }
}