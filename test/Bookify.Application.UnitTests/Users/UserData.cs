using Pft.Domain.Entities.Users;
using Pft.Domain.Entities.Users.ValueObjects;

namespace Pft.Application.UnitTests.Users;
internal static class UserData
{
    public static User Create() => User.Create(FirstName, LastName, Email);

    public static readonly string FirstName = "First";
    public static readonly string LastName = "Last";
    public static readonly Email Email = new("test@test.com");
}
