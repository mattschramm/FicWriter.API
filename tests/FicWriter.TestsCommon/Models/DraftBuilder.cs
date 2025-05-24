using Bogus;
using FicWriter.API.Models;

namespace CommonTestUtils.Models;

public static class DraftBuilder
{
    public static Draft Build(Work work, uint order = 1u, int id = 1)
    {
        return new Faker<Draft>()
            .RuleFor(d => d.Id, id)
            .RuleFor(d => d.WorkId, work.Id)
            .RuleFor(d => d.Work, work)
            .RuleFor(d => d.Title, f => f.Lorem.Word())
            .RuleFor(d => d.Order, order);
    }

    public static List<Draft> BuildList(Work work, uint order = 1u, int count = 3)
    {
        var drafts = new List<Draft>();

        for (var i = 0; i < count; i++)
        {
            drafts.Add(Build(work, order, i));
            order++;
        }

        return drafts;
    }
}
