using ErrorOr;
using MediatR;

namespace FicWriter.API.Features.Works.Dashboard;

public record GetDashboardCommand() : IRequest<ErrorOr<GetDashboardResponse>>;

public record GetDashboardResponse(string Id, string Title, string ShortDescription, DateTime UpdatedAt);

/*public class GetDashboardCommandHandler : IRequestHandler<GetDashboardCommand, ErrorOr<GetDashboardResponse>>
{
    public Task<ErrorOr<GetDashboardResponse>> Handle(GetDashboardCommand request, CancellationToken cancellationToken) => throw new NotImplementedException();
}*/
