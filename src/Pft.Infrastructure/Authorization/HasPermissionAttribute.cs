using Microsoft.AspNetCore.Authorization;

namespace Pft.Infrastructure.Authorization;
public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission);
