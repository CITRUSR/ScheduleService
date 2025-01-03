using Mapster;
using MediatR;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Responses;

namespace ScheduleService.Application.CQRS.ColorEntity.Queries.GetColors;

public class GetColorsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetColorsQuery, List<ColorViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<ColorViewModel>> Handle(
        GetColorsQuery request,
        CancellationToken cancellationToken
    )
    {
        var colors = await _unitOfWork.ColorRepository.GetAllAsync();

        return colors.Adapt<List<ColorViewModel>>();
    }
}
