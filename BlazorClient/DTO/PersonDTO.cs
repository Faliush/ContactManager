using BlazorClient.DTO.Base;
using BlazorClient.DTO.Results;

namespace BlazorClient.DTO;

public class PersonDTO : BaseDTO
{
    public PersonResult? Result { get; set; }
}
