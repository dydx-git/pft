using System.Net.Http.Json;
using Pft.Application.Abstractions.Authentication;
using Pft.Domain.Entities.Users;
using Pft.Infrastructure.Authentication.Models;

namespace Pft.Infrastructure.Authentication;
internal sealed class AuthenticationService(HttpClient httpClient) : IAuthenticationService
{
    private const string PasswordCredentialType = "password";

    public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials =
        [
            new()
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        ];

        var response = await httpClient.PostAsJsonAsync(
            "users",
            userRepresentationModel,
            cancellationToken);

        return ExtractIdentityIdFromLocationHeader(response);
    }

    private string ExtractIdentityIdFromLocationHeader(HttpResponseMessage response)
    {
        const string usersSegmentName = "users/";

        var locationHeader = response.Headers.Location?.PathAndQuery;

        if (locationHeader is null) throw new InvalidOperationException("Location header can't be null");

        var userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        return locationHeader.Substring(
            userSegmentValueIndex + usersSegmentName.Length);        
    }
}
