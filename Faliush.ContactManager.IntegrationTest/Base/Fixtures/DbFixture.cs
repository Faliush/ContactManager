using Faliush.ContactManager.Models;
using Faliush.ContactManager.Models.Base;

namespace Faliush.ContactManager.IntegrationTests.Base.Fixtures;

public static class DbFixture
{
    public static List<Country> Countries =>
        new List<Country>
        {
            new Country { Id = Guid.Parse("6EC9D985-7615-487C-9D73-93F47D6BA36B"), Name = "country1"},
            new Country { Id = Guid.Parse("ABEE88DF-5F40-4F74-A2AF-A734CD346562"), Name = "country2"}
        };

    public static  List<Person> People =>
        new List<Person>
        {
            new Person
            {
                Id = Guid.Parse("A1852277-9385-488F-92D7-0A08938632BE"),
                FirstName = "name1",
                LastName = "surname1",
                DateOfBirth = DateTime.Now,
                Email = "email1@gmail.com",
                Phone = "1111111111",
                Address = "address1",
                CountryId = Guid.Parse("6EC9D985-7615-487C-9D73-93F47D6BA36B"),
                Gender = GenderOptions.None,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "developer",
                UpdatedBy = "developer"
            },
            new Person
            {
                Id = Guid.Parse("27A78540-7137-412D-A541-8EE27C1EEF79"),
                FirstName = "name12",
                LastName = "surname2",
                DateOfBirth = DateTime.Now,
                Email = "email2@gmail.com",
                Phone = "2222222222",
                Address = "address2",
                CountryId = Guid.Parse("ABEE88DF-5F40-4F74-A2AF-A734CD346562"),
                Gender = GenderOptions.None,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "developer",
                UpdatedBy = "developer"
            }
        };
}
