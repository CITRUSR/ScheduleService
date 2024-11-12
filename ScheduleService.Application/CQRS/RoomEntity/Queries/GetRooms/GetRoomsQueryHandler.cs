using MediatR;
using ScheduleService.Application.Common.Models;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Queries.GetRooms;

public class GetRoomsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetRoomsQuery, PagedList<Room>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedList<Room>> Handle(
        GetRoomsQuery request,
        CancellationToken cancellationToken
    )
    {
        var rooms = await _unitOfWork.RoomRepository.GetAsync(
            request.Filter,
            request.PaginationParameters
        );

        return rooms;
    }
}
