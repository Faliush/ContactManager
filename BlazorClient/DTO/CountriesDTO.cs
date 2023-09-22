using BlazorClient.DTO.Base;
using BlazorClient.DTO.Models;


namespace BlazorClient.DTO;

public class CountriesDTO : BaseDTO
{
    public List<CountryModel>? Result { get; set; }
}
