﻿using Pft.Application.Abstractions.Messaging;

namespace Pft.Application.Users.RegisterUser;
public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand<Guid>;
