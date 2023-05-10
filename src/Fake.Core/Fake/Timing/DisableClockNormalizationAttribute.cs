namespace Fake.Timing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
public class DisableClockNormalizationAttribute: Attribute
{
    
}