using Microsoft.EntityFrameworkCore;

namespace Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;

public static class IQueryblePagedListExtention
{
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int pageIndex, 
        int pageSize,
        int indexFrom = 0,
        CancellationToken cancellationToken = default)
    {
        if (indexFrom > pageIndex)
            throw new ArgumentException($"indexFrom can't be larger than pageIndex, indexFrom: {indexFrom} > pageIndex: {pageIndex}");

        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageIndex - indexFrom) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);

        var pagedList = new PagedList<T>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            IndexFrom = indexFrom,
            TotalCount = count,
            Items = items,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        return pagedList;
    } 
}
