namespace FicWriter.API.Endpoints;

public interface IGroupedEndpoint
{
    void MapEndpoint(RouteGroupBuilder app);
}
