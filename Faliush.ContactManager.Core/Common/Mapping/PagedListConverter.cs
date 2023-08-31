using AutoMapper;
using Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;

namespace Faliush.ContactManager.Core.Common.Mapping;

public class PagedListConverter<TMapFrom, TMapTo> : ITypeConverter<IPagedList<TMapFrom>, IPagedList<TMapTo>>
{
    public IPagedList<TMapTo> Convert(IPagedList<TMapFrom> source, IPagedList<TMapTo> destination, ResolutionContext context)
    {
        var collection = context.Mapper.Map<IEnumerable<TMapFrom>, IEnumerable<TMapTo>>(source.Items);

        return new PagedList<TMapTo>(collection, source.PageIndex, source.PageSize, source.IndexFrom);
    }
}
