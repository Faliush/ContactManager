using BlazorClient.DTO.Models;

namespace BlazorClient.DTO.Results;

public class GetForCreatePersonResult
{
    public List<CountryModel> Countries { get; set; } = null!;
	public List<string> Genders { get; set; } = null!;
}
