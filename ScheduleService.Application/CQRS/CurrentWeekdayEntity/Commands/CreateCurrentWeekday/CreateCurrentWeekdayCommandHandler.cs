using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;

public class CreateCurrentWeekdayCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCurrentWeekdayCommand, CurrentWeekday>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CurrentWeekday> Handle(
        CreateCurrentWeekdayCommand request,
        CancellationToken cancellationToken
    )
    {
        var currentWeekday = new CurrentWeekday()
        {
            Color = request.Color,
            Interval = request.Interval,
            UpdatedAt = request.UpdateTime,
        };

        try
        {
            var createdCurrentWeekday = await _unitOfWork.CurrentWeekdayRepository.InsertAsync(
                currentWeekday
            );

            _unitOfWork.CommitTransaction();

            return createdCurrentWeekday;
        }
        catch (NpgsqlException ex)
        {
            _unitOfWork.RollbackTransaction();

            if (ex.SqlState == "P0001")
            {
                throw new CurrentWeekdayAlreadyExistsException();
            }
            throw;
        }
    }
}
