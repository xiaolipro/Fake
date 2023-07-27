using System;

namespace Fake.ObjectMapping.Models;

public class MyEntity
{
    public Guid Id { get; set; }

    public int Number { get; set; }

    public DateTime CreateTime { get; set; }
}