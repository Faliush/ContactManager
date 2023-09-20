using BlazorClient.DTO.Base;
using BlazorClient.DTO.Results;

namespace BlazorClient.DTO;

public class GetForCreatePersonDTO : BaseDTO
{
    public GetForCreatePersonResult? Result { get; set; }
}
