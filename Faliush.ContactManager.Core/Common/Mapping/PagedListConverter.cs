using AutoMapper;
using Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;

namespace Faliush.ContactManager.Core.Common.Mapping;

public class PagedListConverter<TMapFrom, TMapTo> : ITypeConverter<IPagedList<TMapFrom>, IPagedList<TMapTo>>
{
    public IPagedList<TMapTo> Convert(IPagedList<TMapFrom> source, IPagedList<TMapTo> destination, ResolutionContext context) =>
           // ReSharper disable once ConditionIsAlwaysTrueOrFalse
           source == null
               ? PagedList.Empty<TMapTo>()
               : PagedList.From(source, items => context.Mapper.Map<IEnumerable<TMapTo>>(items));
}
