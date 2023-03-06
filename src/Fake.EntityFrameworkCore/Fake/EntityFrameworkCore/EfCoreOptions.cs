namespace Fake.EntityFrameworkCore;

public class EfCoreOptions
{
    public DatabaseProvider DatabaseProvider { get; set; }
    public string ConnectionString { get; set; }
}