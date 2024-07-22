using System.ComponentModel.DataAnnotations;
using Fake.Application;
using Fake.Auditing;
using Fake.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWebDemo;

[Audited]
public class AppsService : ApplicationService
{
    [HttpPost("app/getname")]
    public virtual string GetName(string lastName, string firstName)
    {
        Logger.LogError("突突你d");
        return $"{firstName} {lastName}";
    }

    /// <summary>
    /// 生成单个随机数字
    /// </summary>
    public virtual int CreateNum()
    {
        Random random = new Random();
        int num = random.Next(10);
        return num;
    }

    public virtual void GetError(Student student)
    {
        throw new Exception();
    }

    public virtual Task<Student> CreateError(Student student)
    {
        throw new DomainException("Demo:GG");
    }
}

public class Student
{
    /// <summary>
    /// 学生姓名
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 学生年龄
    /// </summary>
    [Range(18, 60)]
    public int Age { get; set; }
}