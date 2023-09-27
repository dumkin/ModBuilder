using System.Collections.Generic;

namespace Curse.Integration.Models;

public class GenericListResponse<T>
{
    public List<T> Data { get; set; } = new();
    public Pagination Pagination { get; set; }
}