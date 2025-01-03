using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.DeleteRoom;

public class DeleteRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteRoomCommand, Room>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Room> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        // var room = await _unitOfWork.RoomRepository.GetByIdAsync(request.Id);


        var room = await _unitOfWork.RoomRepository.DeleteAsync(request.Id);

        if (room == null)
        {
            throw new RoomNotFoundException(request.Id);
        }

        _unitOfWork.CommitTransaction();

        return room;
    }
}
