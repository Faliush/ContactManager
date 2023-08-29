namespace Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;

public static class IEnumerablePagedListExtention
{
    public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int indexFrom = 0) =>
        new PagedList<T>(source, pageIndex, pageSize, indexFrom);
}
