using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.UpdateRoom;

public class UpdateRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRoomCommand, Room>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Room> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var newRoom = new Room()
        {
            Id = request.Id,
            Name = request.Name,
            FullName = request.FullName
        };

        var updatedRoom = await _unitOfWork.RoomRepository.UpdateAsync(newRoom);

        if (updatedRoom == null)
        {
            throw new RoomNotFoundException(request.Id);
        }

        _unitOfWork.CommitTransaction();

        return updatedRoom;
    }
}
