namespace TimMovie.Core.Classes;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    
    public int PageSize { get; set; }

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public PaginatedList()
    {
        
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(
        IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip(
                (pageIndex - 1) * pageSize)
            .Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}