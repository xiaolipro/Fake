using Fake.Localization;

namespace Fake.TenantManagement.Domain.Localization;

[LocalizationResourceName("FakeTenantManagement")]
public class FakeTenantManagementResource
{
    public static string DefaultPrefix = "Fake.TenantManagement";
    public static string TenantNameDuplicate = DefaultPrefix + ":TenantNameDuplicate";
}