namespace FicWriter.API.Features.Users.Profile;

public static class GetProfileMapper
{
    public static UserProfileResponse ToProfileResponse(this Models.User user)
        => new(user.Name, user.Email);
}
