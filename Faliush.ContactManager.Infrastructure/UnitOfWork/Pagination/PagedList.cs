namespace Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;

public class PagedList<T> : IPagedList<T>
{
    public int IndexFrom { get; init; }

    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages { get; init; }

    public IList<T> Items { get; init; }

    public bool HasPreviousPage => PageIndex - IndexFrom > 0;

    public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

    public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int indexFrom)
    {
        if (indexFrom > pageIndex)
            throw new ArgumentException($"indexFrom can't be larger than pageIndex, indexFrom: {indexFrom} > pageIndex: {pageIndex}");

        if(source is IQueryable<T> queryable)
        { 
            PageIndex = pageIndex;
            IndexFrom = indexFrom;
            PageSize = pageSize;
            TotalCount = queryable.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = queryable.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize)
                .ToList();
        }
        else
        {
            var enumerable = source.ToList();
            PageIndex = pageIndex;
            PageSize = pageSize;
            IndexFrom = indexFrom;
            TotalCount = enumerable.Count;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = enumerable.Skip((PageIndex - IndexFrom) * PageSize).Take(PageSize)
                .ToList();
        }
    }

    public PagedList() => Items = Array.Empty<T>();


}
