using MediatR;
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

        var insertedColor = await _unitOfWork.ColorRepository.InsertAsync(color);

        _unitOfWork.CommitTransaction();

        return insertedColor;
    }
}
