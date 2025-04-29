using FicWriter.API.Models;

namespace FicWriter.API.Features.Works.Create;

public static class CreateWorkMapper
{
    public static CreateWorkCommand ToCommand(this CreateWorkRequest request) => new(request.Title, request.Description);

    public static Work ToWork(this CreateWorkCommand command, long userId) =>
        new()
        {
            Title = command.Title,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserId = userId,
        };

    public static CreateWorkResponse ToResponse(this Work work) => new(work.Id, work.Title);
}
