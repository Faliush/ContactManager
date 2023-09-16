using BlazorClient.DTO.Base;
using BlazorClient.DTO.Results;

namespace BlazorClient.DTO;

public class PeopleDTO : BaseDTO
{
    public PeopleResult? Result { get; set; }
}
