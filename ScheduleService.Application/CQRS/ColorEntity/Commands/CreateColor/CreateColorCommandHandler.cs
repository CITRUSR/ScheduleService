using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.CreateColor;

public class CreateColorCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateColorCommand, Color>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Color> Handle(CreateColorCommand request, CancellationToken cancellationToken)
    {
        Color color = new() { Name = request.Name };

        Color insertedColor;

        try
        {
            insertedColor = await _unitOfWork.ColorRepository.InsertAsync(color);

            _unitOfWork.CommitTransaction();
        }
        catch (NpgsqlException ex) when (ex.SqlState == "23505")
        {
            _unitOfWork.RollbackTransaction();

            throw new ColorNameAlreadyExistsException(request.Name);
        }
        catch
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }

        return insertedColor;
    }
}
