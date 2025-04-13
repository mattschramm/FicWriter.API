using ErrorOr;
using FicWriter.API.Models;
using MediatR;

namespace FicWriter.API.Features.Users.Create;

public sealed record CreateUserCommand(string Name, string Email, string Password) : IRequest<ErrorOr<CreateUserResponse>>;
public sealed record CreateUserResponse(long Id, string Name, AccessToken AccessToken);
public sealed record CreateUserRequest(string Name, string Email, string Password);
