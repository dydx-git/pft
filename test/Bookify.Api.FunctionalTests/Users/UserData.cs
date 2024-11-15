using Bookify.API.Controllers.Users;

namespace Bookify.Api.FunctionalTests.Users;
public static class UserData
{
    public static readonly RegisterUserRequest RegisterTestUserRequest = new("test@test.com", "test", "test", "12345");
}
