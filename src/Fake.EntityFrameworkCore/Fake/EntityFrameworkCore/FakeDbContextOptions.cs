using System;
using Microsoft.EntityFrameworkCore;

namespace Fake.EntityFrameworkCore;

public class FakeDbContextOptions<TDbContext> :DbContextOptions<TDbContext> where TDbContext : DbContext
{
    public Type UserIdType { get; set; }
}