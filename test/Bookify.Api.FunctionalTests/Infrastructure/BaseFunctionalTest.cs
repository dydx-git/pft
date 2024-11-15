﻿using System.Net.Http.Json;
using Bookify.API.Controllers.Users;
using Bookify.Api.FunctionalTests.Users;
using Pft.Application.Users.LogInUser;

namespace Bookify.Api.FunctionalTests.Infrastructure;
public abstract class BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient HttpClient = factory.CreateClient();

    protected async Task<string> GetAccessToken()
    {
        HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync(
            "api/v1/users/login",
            new LogInUserRequest(
                UserData.RegisterTestUserRequest.Email,
                UserData.RegisterTestUserRequest.Password));

        var accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();

        return accessTokenResponse!.AccessToken;
    }
}
