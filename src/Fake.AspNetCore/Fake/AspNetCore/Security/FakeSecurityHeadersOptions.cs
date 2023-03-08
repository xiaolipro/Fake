namespace Fake.AspNetCore.Security;

public class FakeSecurityHeadersOptions
{
    public bool UseContentSecurityPolicyHeader { get; set; }

    public string ContentSecurityPolicyValue { get; set; }


    public FakeSecurityHeadersOptions()
    {
        // object-src 'none' 禁止加载引用不存在的资源；
        // form-action 'self' 只允许表单提交到当前域名；
        // frame-ancestors 'none' 预防攻击者对你的页面进行 Click-jacking 攻击，限制当前页面不允许嵌套在任何 frame/iframe 中
        // 保证用户打开网站时无法通过 iframe 方式展示其他内容和欺骗行为。
        ContentSecurityPolicyValue = "object-src 'none'; form-action 'self'; frame-ancestors 'none'";
    }
}