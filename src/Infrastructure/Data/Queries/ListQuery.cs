using Microsoft.EntityFrameworkCore;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public static class ListQuery
{
    public static async Task<IEnumerable<T>> ListAsync<T>(
        AppDbContext db,
        IQueryable<T> query,
        int? skip = null,
        int? take = null
    )
    {
        if (skip is not null)
            query = query.Skip((int)skip);
        if (take is not null)
            query = query.Take((int)take);

        var result = await query.ToListAsync();

        return result;
    }
}