using BlazorClient.DTO.Models;

namespace BlazorClient.DTO.Results;

public class PeopleResult
{
    public int IndexFrom { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<PeopleModel>? Items { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
