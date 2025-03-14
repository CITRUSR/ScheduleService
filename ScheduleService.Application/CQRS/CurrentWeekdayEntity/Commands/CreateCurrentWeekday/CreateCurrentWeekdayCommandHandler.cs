using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Queries.GetColors;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;

public class CreateCurrentWeekdayCommandHandler(
    IUnitOfWork unitOfWork,
    IPublisher publisher,
    IMediator mediator
) : IRequestHandler<CreateCurrentWeekdayCommand, CurrentWeekday>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPublisher _publisher = publisher;
    private readonly IMediator _mediator = mediator;

    public async Task<CurrentWeekday> Handle(
        CreateCurrentWeekdayCommand request,
        CancellationToken cancellationToken
    )
    {
        var colors = await _mediator.Send(new GetColorsQuery(), cancellationToken);

        colors.EnsureColorExists(request.Color);

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

            await _publisher.Publish(
                new CreateCurrentWeekdayEvent
                {
                    Color = request.Color,
                    Interval = request.Interval,
                    UpdateTime = request.UpdateTime
                }
            );

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
