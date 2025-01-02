using MediatR;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.Common.Services;

public class ClassService(IUnitOfWork unitOfWork, IMediator mediator) : IClassService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMediator _mediator = mediator;

    public async Task<(
        List<ColorClassesDto<TClassDetail>>,
        Weekday weekday
    )> GetClassesForDay<TClassDetail>(IClassSpecification specification, int weekdayOrder)
        where TClassDetail : ClassDetailBase
    {
        //order corresponds to weekday id
        var weekday = await _mediator.Send(new GetWeekdayByIdQuery(weekdayOrder));

        var classes = await _unitOfWork.ClassRepository.GetAsync(specification);

        var colorClassesDto = classes.ToColorClasses<TClassDetail>();

        colorClassesDto.CountClassOrder();

        return (colorClassesDto, weekday);
    }
}
