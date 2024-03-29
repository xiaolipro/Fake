﻿using System;
using System.Security.Claims;

namespace Fake.Users;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    /// <summary>
    /// 用户唯一标识
    /// </summary>

    public Guid? Id { get; }

    /// <summary>
    /// 获取当前用户的名称。
    /// </summary>

    public string? UserName { get; }

    Claim? FindClaimOrNull(string claimType);
}