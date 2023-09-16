namespace BlazorClient.DTO.Base;

public abstract class BaseDTO
{
    public bool Ok { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public Dictionary<string, object>? Exception { get; set; }
}
