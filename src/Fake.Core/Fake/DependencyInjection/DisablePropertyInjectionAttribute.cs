namespace Fake.DependencyInjection;


/// <summary>
/// 禁用属性注入
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class DisablePropertyInjectionAttribute:Attribute
{
}