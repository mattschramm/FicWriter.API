using ErrorOr;
using FicWriter.API.Infrastructure.Services;
using MediatR;

namespace FicWriter.API.Features.Users.Profile;

public record GetProfileCommand() : IRequest<ErrorOr<UserProfileResponse>>;
public record UserProfileResponse(string Name, string Email);

public class GetProfileCommandHandler : IRequestHandler<GetProfileCommand, ErrorOr<UserProfileResponse>>
{
    private readonly ICurrentUser _currentUser;

    public GetProfileCommandHandler(ICurrentUser currentUser) => _currentUser = currentUser;

    public async Task<ErrorOr<UserProfileResponse>> Handle(GetProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        return user.ToProfileResponse();
    }
}
