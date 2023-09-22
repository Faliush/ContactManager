using System.ComponentModel.DataAnnotations;

namespace BlazorClient.DTO;

public class CreateCountryDTO
{
	[Required]
	[MinLength(2)]
	[MaxLength(40)]
	public string Name { get; set; } = null!;
}
