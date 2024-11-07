using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Queries.GetRoomById;

public class GetRoomByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetRoomByIdQuery, Room>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Room> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(request.Id);

        if (room == null)
        {
            throw new RoomNotFoundException(request.Id);
        }

        return room;
    }
}
