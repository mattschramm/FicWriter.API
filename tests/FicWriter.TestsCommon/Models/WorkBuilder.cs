using Bogus;
using FicWriter.API.Models;

namespace CommonTestUtils.Models;

public static class WorkBuilder
{
    public static Work Build(User user)
    {
        return new Faker<Work>()
            .RuleFor(w => w.Id, f => 1)
            .RuleFor(w => w.Title, f => f.Lorem.Sentence(1))
            .RuleFor(w => w.Description, f => f.Lorem.Paragraph(1))
            .RuleFor(w => w.UserId, f => user.Id)
            .RuleFor(w => w.User, f => user);
    }
}
