﻿using Pft.Domain.Entities.Users;

namespace Pft.Infrastructure.Authorization;

public sealed class UserRolesResponse
{
    public Guid Id { get; init; }
    public List<Role> Roles { get; init; } = [];
}
