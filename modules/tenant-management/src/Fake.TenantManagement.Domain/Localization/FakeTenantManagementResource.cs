using Fake.Localization;

namespace Fake.TenantManagement.Domain.Localization;

[LocalizationResourceName(DefaultPrefix)]
public class FakeTenantManagementResource
{
    private const string DefaultPrefix = "Fake.TenantManagement";
    public static string TenantNameDuplicate = DefaultPrefix + ":TenantNameDuplicate";
}