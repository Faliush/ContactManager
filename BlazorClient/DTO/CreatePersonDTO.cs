using BlazorClient.DTO.Base;
using BlazorClient.DTO.Results;

namespace BlazorClient.DTO;

public class CreatePersonDTO : BaseDTO
{
    public CreatePersonResult? Result { get; set; }
}
