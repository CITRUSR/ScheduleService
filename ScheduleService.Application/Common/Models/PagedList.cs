namespace ScheduleService.Application.Common.Models;

public class PagedList<T>
{
    public List<T> Items { get; set; }
    public int Count { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int LastPage { get; }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        Count = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        LastPage = (int)Math.Ceiling(count / (double)pageSize);
    }
}
