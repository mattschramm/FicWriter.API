using FicWriter.API.Enums;
using FicWriter.API.Features.Works.Dashboard;
using FicWriter.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FicWriter.API.Infrastructure.Data.Repositories.Works;

public class WorkRepository(FicWriterDbContext dbContext) : IWorkRepository
{
    private readonly FicWriterDbContext _dbContext = dbContext;

    public async Task Create(Work work)
    {
        await _dbContext.Works.AddAsync(work);
    }

    public async Task Delete(long id)
    {
        var work = await _dbContext.Works.FindAsync(id);
        
        if (work is not null)
        {
            work.IsActive = false;
            work.DeletedAt = DateTime.UtcNow;
        }
    }

    public Task<bool> Exists(User user, long id) => _dbContext.Works
        .AsNoTracking()
        .AnyAsync(w => w.Id == id && w.UserId == user.Id && w.IsActive);

    public Task<List<Work>> GetAllWorks(User user, long id, bool withTracking = false)
    {
        var query = _dbContext.Works
            .AsNoTracking()
            .Include(w => w.Drafts)
            .Include(w => w.Genres)
            .Include(w => w.Tags)
            .Where(w => w.UserId == user.Id && w.IsActive);
        
        if (withTracking)
        {
            query = query.AsTracking();
        }
        
        return query
            .OrderByDescending(w => w.UpdatedAt)
            .ToListAsync();
    }
    
    public async Task<List<Work>> GetArchivedWorks(User user, long id, bool withTracking = false)
    {
        var query = _dbContext.Works
            .AsNoTracking()
            .Include(w => w.Drafts)
            .Include(w => w.Genres)
            .Include(w => w.Tags)
            .Where(w => w.UserId == user.Id && w.IsActive && w.IsArchived);

        if (withTracking)
        {
            query = query.AsTracking();
        }

        return await query
            .OrderByDescending(w => w.UpdatedAt)
            .ToListAsync();
    }
    
    public async Task<Work?> GetById(User user, long id) => await _dbContext.Works
        .AsNoTracking()
        .Include(w => w.Drafts)
        .FirstOrDefaultAsync(w => w.Id == id && w.UserId == user.Id && w.IsActive && !w.IsArchived);
    
    public async Task<List<Work>> GetDashboard(User user, GetDashboardCommand command)
    {
        var query = _dbContext.Works
            .AsNoTracking()
            .Include(w => w.Drafts)
            .Include(w => w.Genres)
            .Include(w => w.Tags)
            .Where(w => w.UserId == user.Id && w.IsActive && !w.IsArchived);
        
        if (!string.IsNullOrEmpty(command.Title))
        {
            var trimmedTitle = command.Title.Trim();
            query = query.Where(w => w.Title.Contains(trimmedTitle, StringComparison.OrdinalIgnoreCase));
        }

        if (command.Genres is not null and { Length: > 0 })
        {
            query = query.Where(w => w.Genres.Any(g => command.Genres.Contains(g.GenreType)));
        }

        if (command.Tags is not null and { Length: > 0 })
        {
            query = query.Where(w => w.Tags.Any(t => command.Tags.Contains(t.Content)));
        }

        query = command.Order switch
        {
            Orders.LastUpdated => query.OrderByDescending(w => w.UpdatedAt),
            Orders.LastCreated => query.OrderByDescending(w => w.CreatedAt),
            Orders.FirstCreated => query.OrderBy(w => w.CreatedAt),
            Orders.FirstUpdated => query.OrderBy(w => w.UpdatedAt),
            Orders.Alphabetical => query.OrderBy(w => w.Title),
            Orders.AlphabeticalReverse => query.OrderByDescending(w => w.Title),
            _ => query.OrderByDescending(w => w.UpdatedAt)
        };
        
        return await query
            .Skip((command.Page - 1) * command.PageSize)
            .Take(command.PageSize)
            .ToListAsync();
    }

    public async Task<Work?> GetWorkByIdWithTracking(User user, long id, bool includeArchived = false)
    {
        var query = _dbContext.Works
            .Include(w => w.Drafts)
            .Include(w => w.Genres)
            .Include(w => w.Tags)
            .Where(w => w.UserId == user.Id && w.IsActive);

        if (!includeArchived)
        {
            query = query.Where(w => !w.IsArchived);
        }

        return await query
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public void Update(Work work) => _dbContext.Works.Update(work);
}
