using ErrorOr;
using FicWriter.API.Enums;
using FicWriter.API.Infrastructure.Data.Repositories.Works;
using FicWriter.API.Infrastructure.Errors;
using FicWriter.API.Infrastructure.Services;
using FicWriter.API.Shared.Draft;
using MediatR;

namespace FicWriter.API.Features.Works.Get;

public record GetWorkByIdCommand(long Id) : IRequest<ErrorOr<GetWorkByIdResponse>>;

public record GetWorkByIdResponse(string Id, string Title, string Description, DateTime CreatedAt, DateTime UpdatedAt,
                                  List<DraftResponse> Drafts, List<Genres> Genres, List<string> Tags);

public class GetWorkByIdCommandHandler(IWorkRepository repository, ICurrentUser currentUser, GetWorkByIdMapper mapper) : IRequestHandler<GetWorkByIdCommand, ErrorOr<GetWorkByIdResponse>>
{
    private readonly IWorkRepository _repository = repository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly GetWorkByIdMapper _mapper = mapper;

    public async Task<ErrorOr<GetWorkByIdResponse>> Handle(GetWorkByIdCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _currentUser.GetCurrentUser();

        var work = await _repository.GetById(currentUser, request.Id);

        if (work is null)
            return WorkErrors.WorkNotFound();

        return _mapper.ToResponse(work);
    }
}
