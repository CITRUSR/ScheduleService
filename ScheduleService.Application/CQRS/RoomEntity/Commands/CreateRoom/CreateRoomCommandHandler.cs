using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.CreateRoom;

public class CreateRoomCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRoomCommand, Room>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Room> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room { FullName = request.FullName, Name = request.Name };

        Room createdRoom;

        try
        {
            createdRoom = await _unitOfWork.RoomRepository.InsertAsync(room);

            _unitOfWork.CommitTransaction();
        }
        catch (NpgsqlException ex) when (ex.SqlState == "23505")
        {
            _unitOfWork.RollbackTransaction();
            throw new RoomNameAlreadyExistsException(request.Name);
        }
        catch
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }

        return createdRoom;
    }
}
