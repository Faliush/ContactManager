using BlazorClient.DTO.Base;
using BlazorClient.DTO.Results;

namespace BlazorClient.DTO;

public class UpdatePersonDTO : BaseDTO
{
    public UpdatePersonResult? Result { get; set; }
}
