namespace FicWriter.API.Infrastructure.Data;

public class UnitOfWork(FicWriterDbContext context) : IUnitOfWork
{
    private readonly FicWriterDbContext _context = context;

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}
