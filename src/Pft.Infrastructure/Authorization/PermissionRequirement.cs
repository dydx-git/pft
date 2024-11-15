﻿using Microsoft.AspNetCore.Authorization;

namespace Pft.Infrastructure.Authorization;

internal sealed class PermissionRequirement(string permissions) : IAuthorizationRequirement
{
    public string Permissions { get; } = permissions;
}