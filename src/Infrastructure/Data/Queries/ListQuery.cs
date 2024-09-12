using Microsoft.EntityFrameworkCore;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public static class ListQuery
{
    public static async Task<(IEnumerable<T>, int)> ListAsync<T>(
        IQueryable<T> query,
        int? skip = null,
        int? take = null
    )
    {
        int count = query.Count();
        if (skip is not null)
            query = query.Skip((int)skip);
        if (take is not null)
            query = query.Take((int)take);

        var result = await query.ToListAsync();

        return (result, count);
    }
}
