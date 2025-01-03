using Mapster;
using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Responses;

namespace ScheduleService.Application.CQRS.ColorEntity.Queries.GetColorById;

public class GetColorByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetColorByIdQuery, ColorViewModel>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ColorViewModel> Handle(
        GetColorByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var color = await _unitOfWork.ColorRepository.GetByIdAsync(request.Id);

        if (color == null)
        {
            throw new ColorNotFoundException(request.Id);
        }

        return color.Adapt<ColorViewModel>();
    }
}
