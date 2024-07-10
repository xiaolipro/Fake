using Fake.Localization;

namespace Fake.TenantManagement.Domain.Localization;

[LocalizationResourceName(DefaultPrefix)]
public class FakeTenantManagementResource
{
    public const string DefaultPrefix = "Fake.TenantManagement";
    public const string TenantNameDuplicate = DefaultPrefix + ":TenantNameDuplicate";
    public const string TenantNotExists = DefaultPrefix + ":TenantNotExists";
}