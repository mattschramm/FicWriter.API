using Bogus;
using FicWriter.API.Models;

namespace CommonTestUtils.Models;

public static class DraftBuilder
{
    public static Draft Build(long workId = 1, uint order = 1u)
    {
        return new Faker<Draft>()
            .RuleFor(d => d.Id, 1)
            .RuleFor(d => d.WorkId, workId)
            .RuleFor(d => d.Title, f => f.Lorem.Word())
            .RuleFor(d => d.Order, order);
    }
}
