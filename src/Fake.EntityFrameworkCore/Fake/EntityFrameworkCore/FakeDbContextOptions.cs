using System;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore;

public class FakeDbContextOptions<TDbContext> :DbContextOptions<TDbContext> where TDbContext : DbContext
{
    override onc
    public Type UserIdType { get; set; }
}