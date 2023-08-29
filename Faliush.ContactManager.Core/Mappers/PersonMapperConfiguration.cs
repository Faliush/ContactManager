using AutoMapper;
using Faliush.ContactManager.Core.Common.Mapping;
using Faliush.ContactManager.Core.Logic.PersonLogic.ViewModels;
using Faliush.ContactManager.Infrastructure.UnitOfWork.Pagination;
using Faliush.ContactManager.Models;
using Microsoft.AspNetCore.Identity;

namespace Faliush.ContactManager.Core.Mappers;

public class PersonMapperConfiguration : Profile
{
	public PersonMapperConfiguration()
	{
		CreateMap<Person, PeopleViewModel>()
			.ForMember(des => des.Id, src => src.MapFrom(x => x.Id))
			.ForMember(des => des.FirstName, src => src.MapFrom(x => x.FirstName))
			.ForMember(des => des.LastName, src => src.MapFrom(x => x.LastName))
			.ForMember(des => des.Email, src => src.MapFrom(x => x.Email));


        CreateMap<Person, PersonViewModel>()
			.ForMember(des => des.Id, src => src.MapFrom(x => x.Id))
			.ForMember(des => des.FirstName, src => src.MapFrom(x => x.FirstName))
			.ForMember(des => des.LastName, src => src.MapFrom(x => x.LastName))
			.ForMember(des => des.Email, src => src.MapFrom(x => x.Email))
			.ForMember(des => des.Phone, src => src.MapFrom(x => x.Phone))
			.ForMember(des => des.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth))
			.ForMember(des => des.Address, src => src.MapFrom(x => x.Address))
			.ForMember(des => des.CountryName, src => src.MapFrom(x => x.Country.Name))
			.ForMember(des => des.Gender, src => src.MapFrom(x => x.Gender.ToString()))
			.ForMember(des => des.Age, src => src.Ignore());

		CreateMap<PersonCreateViewModel, Person>()
			.ForMember(des => des.Id, src => src.Ignore())
			.ForMember(des => des.FirstName, src => src.MapFrom(x => x.FirstName))
			.ForMember(des => des.LastName, src => src.MapFrom(x => x.LastName))
			.ForMember(des => des.Email, src => src.MapFrom(x => x.Email))
			.ForMember(des => des.Phone, src => src.MapFrom(x => x.Phone))
			.ForMember(des => des.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth))
			.ForMember(des => des.Address, src => src.MapFrom(x => x.Address))
			.ForMember(des => des.CountryId, src => src.MapFrom(x => x.CountryId))
			.ForMember(des => des.Gender, src => src.Ignore())
			.ForMember(des => des.Country, src => src.Ignore())
            .ForMember(x => x.CreatedAt, o => o.Ignore())
			.ForMember(x => x.UpdatedAt, o => o.Ignore())
			.ForMember(x => x.UpdatedBy, o => o.Ignore())
			.ForMember(x => x.CreatedBy, o => o.MapFrom((_, _, _, context) => context.Items[nameof(IdentityUser)]));

		CreateMap<Person, PersonUpdateViewModel>()
			.ForMember(des => des.Gender, src => src.MapFrom(x => x.Gender.ToString()));

		CreateMap<PersonUpdateViewModel, Person>()
            .ForMember(des => des.Id, src => src.Ignore())
            .ForMember(des => des.FirstName, src => src.MapFrom(x => x.FirstName))
            .ForMember(des => des.LastName, src => src.MapFrom(x => x.LastName))
            .ForMember(des => des.Email, src => src.MapFrom(x => x.Email))
            .ForMember(des => des.Phone, src => src.MapFrom(x => x.Phone))
            .ForMember(des => des.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth))
            .ForMember(des => des.Address, src => src.MapFrom(x => x.Address))
            .ForMember(des => des.CountryId, src => src.MapFrom(x => x.CountryId))
            .ForMember(des => des.Gender, src => src.Ignore())
            .ForMember(des => des.Country, src => src.Ignore())
            .ForMember(x => x.CreatedAt, o => o.Ignore())
            .ForMember(x => x.UpdatedAt, o => o.Ignore())
            .ForMember(x => x.UpdatedBy, o => o.MapFrom((_,_,_,context) => context.Items[nameof(IdentityUser)]))
            .ForMember(x => x.CreatedBy, o => o.Ignore());


		CreateMap<IPagedList<Person>, IPagedList<PeopleViewModel>>()
			.ConvertUsing<PagedListConverter<Person, PeopleViewModel>>();
    }
}
