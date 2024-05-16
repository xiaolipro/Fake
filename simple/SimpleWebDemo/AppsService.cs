using System.ComponentModel.DataAnnotations;
using Fake;
using Fake.DomainDrivenDesign.Application;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWebDemo;

public class AppsService : ApplicationService
{
    [HttpPost("app/getname")]
    public string GetName(string lastName, string firstName)
    {
        Logger.LogError("突突你d");
        return $"{firstName} {lastName}";
    }

    /// <summary>
    /// 生成单个随机数字
    /// </summary>
    public int CreateNum()
    {
        Random random = new Random();
        int num = random.Next(10);
        return num;
    }

    public void GetError(Student student)
    {
        throw new BusinessException("GG");
    }
}

public class Student
{
    public string Name { get; set; }
    [Range(18, 60)] public int Age { get; set; }
}