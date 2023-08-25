using AutoMapper;
using Faliush.ContactManager.Core.Logic.CountryLogic.ViewModels;
using Faliush.ContactManager.Models;

namespace Faliush.ContactManager.Core.Mappers;

public class CountryMapperConfiguration : Profile
{
	public CountryMapperConfiguration()
	{
		CreateMap<Country, CountryViewModel>();
	}
}
