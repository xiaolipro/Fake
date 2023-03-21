using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppTests.Test;

public class Migratior
{
 
    [Fact]
    public void Migrator()
    {
        new OrderingContext().Database.Migrate();
    }
}