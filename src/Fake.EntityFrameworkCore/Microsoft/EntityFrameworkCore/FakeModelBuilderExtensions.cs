using Fake.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class FakeModelBuilderExtensions
{
    private const string ModelDatabaseProviderAnnotationKey = "_Fake_DatabaseProvider";
    public static void SetDatabaseProvider(
        this ModelBuilder modelBuilder,
        DatabaseProvider databaseProvider)
    {
        modelBuilder.Model.SetAnnotation(ModelDatabaseProviderAnnotationKey, databaseProvider);
    }
}