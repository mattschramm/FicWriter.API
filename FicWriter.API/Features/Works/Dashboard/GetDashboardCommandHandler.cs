using ErrorOr;
using FicWriter.API.Enums;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FicWriter.API.Features.Works.Dashboard;

public record GetDashboardCommand(
    [FromQuery] int Page,
    [FromQuery] int PageSize,
    [FromQuery] string? Title,
    [FromQuery(Name = "genre")] Genres[]? Genres,
    [FromQuery(Name = "tag")] string[]? Tags,
    [FromQuery] Orders? Order) : IRequest<ErrorOr<GetDashboardResponse>>;

public record GetWorksResponse(string Id, string Title, string ShortDescription, 
    DateTime UpdatedAt, List<Genres> Genres, List<string> Tags);

public record GetDashboardResponse(int Total, List<GetWorksResponse> Works);

public class GetDashboardCommandHandler(IWorkRepository workRepository, 
    ICurrentUser currentUser, GetDashboardMapper mapper) : IRequestHandler<GetDashboardCommand, ErrorOr<GetDashboardResponse>>
{
    private readonly IWorkRepository _workRepository = workRepository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly GetDashboardMapper _mapper = mapper;

    public async Task<ErrorOr<GetDashboardResponse>> Handle(GetDashboardCommand request, CancellationToken cancellationToken)
    {
        var user = await _currentUser.GetCurrentUser();

        var works = await _workRepository.GetDashboard(user, request);

        if (works.Count == 0)
        {
            return new GetDashboardResponse(0, []);
        }

        return _mapper.ToResponse(works);
    }
}
