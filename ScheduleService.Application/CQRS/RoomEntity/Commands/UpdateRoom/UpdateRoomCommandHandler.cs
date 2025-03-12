using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.UpdateRoom;

public class UpdateRoomCommandHandler(
    IUnitOfWork unitOfWork,
    IUniqueConstraintExceptionChecker uniqueChecker
) : IRequestHandler<UpdateRoomCommand, Room>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUniqueConstraintExceptionChecker _uniqueChecker = uniqueChecker;

    public async Task<Room> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var newRoom = new Room()
        {
            Id = request.Id,
            Name = request.Name,
            FullName = request.FullName,
        };

        Room updatedRoom;

        try
        {
            updatedRoom = await _unitOfWork.RoomRepository.UpdateAsync(newRoom);

            if (updatedRoom == null)
            {
                throw new RoomNotFoundException(request.Id);
            }

            _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            _unitOfWork.RollbackTransaction();

            var field = _uniqueChecker.Check<Room>(ex);

            if (field != null && field.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                throw new RoomNameAlreadyExistsException(request.Name);
            }

            throw;
        }

        return updatedRoom;
    }
}
