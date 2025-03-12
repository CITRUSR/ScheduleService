using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.CreateRoom;

public class CreateRoomCommandHandler(
    IUnitOfWork unitOfWork,
    IUniqueConstraintExceptionChecker uniqueChecker
) : IRequestHandler<CreateRoomCommand, Room>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUniqueConstraintExceptionChecker _uniqueChecker = uniqueChecker;

    public async Task<Room> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room { FullName = request.FullName, Name = request.Name };

        Room createdRoom;

        try
        {
            createdRoom = await _unitOfWork.RoomRepository.InsertAsync(room);

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

        return createdRoom;
    }
}
