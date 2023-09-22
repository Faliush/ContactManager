using BlazorClient.DTO.Base;
using BlazorClient.DTO.Models;

namespace BlazorClient.DTO;

public class CountryDTO : BaseDTO
{
	public CountryModel? Result { get; set; }
}
