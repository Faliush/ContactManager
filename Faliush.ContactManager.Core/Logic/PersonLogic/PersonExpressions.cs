using Faliush.ContactManager.Core.Enums;
using Faliush.ContactManager.Models;
using System.Linq.Expressions;

namespace Faliush.ContactManager.Core.Logic.PersonLogic;

public static class PersonExpressions
{
    public static Expression<Func<Person, bool>> SearchPredicate(string? searchBy, string? searchString)
    {
        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return x => true;

        Expression<Func<Person, bool>> expression = searchBy switch
        {
            nameof(Person.FirstName) =>
                x => x.FirstName.Contains(searchString),

            nameof(Person.LastName) =>
                x => x.LastName.Contains(searchString),

            nameof(Person.Address) =>
                x => x.Address.Contains(searchString),

            nameof(Person.Email) =>
                x => x.Email.Contains(searchString),

            _ => x => true
        };

        return expression;
    }

    public static Func<IQueryable<Person>, IOrderedQueryable<Person>> OrderBy(string sortBy, SortOptions sortOrder)
    {
        Func<IQueryable<Person>, IOrderedQueryable<Person>> func = (sortBy, sortOrder) switch
        {
            (nameof(Person.FirstName), SortOptions.Asc) => x => x.OrderBy(temp => temp.FirstName),
            (nameof(Person.FirstName), SortOptions.Desc) => x => x.OrderByDescending(temp => temp.FirstName),

            (nameof(Person.LastName), SortOptions.Asc) => x => x.OrderBy(temp => temp.LastName),
            (nameof(Person.LastName), SortOptions.Desc) => x => x.OrderByDescending(temp => temp.LastName),

            (nameof(Person.Email), SortOptions.Asc) => x => x.OrderBy(temp => temp.Email),
            (nameof(Person.Email), SortOptions.Desc) => x => x.OrderByDescending(temp => temp.Email),

            _ => x => x.OrderBy(temp => temp.LastName)
        };

        return func;
    }
}
