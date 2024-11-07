using ScheduleService.Application.Common.Models;

namespace ScheduleService.Application.CQRS.RoomEntity.Queries.GetRooms;

public class RoomFilter
{
    public string? SearchString { get; set; }
    public RoomFilterState FilterBy { get; set; }
    public OrderState OrderState { get; set; }
}
