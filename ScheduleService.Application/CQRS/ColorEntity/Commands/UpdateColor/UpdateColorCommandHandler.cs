using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.UpdateColor;

public class UpdateColorCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateColorCommand, Color>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Color> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
    {
        var newColor = new Color() { Id = request.Id, Name = request.Name };

        var updatedColor = await _unitOfWork.ColorRepository.UpdateAsync(newColor);

        if (updatedColor == null)
        {
            throw new ColorNotFoundException(request.Id);
        }

        _unitOfWork.CommitTransaction();

        return updatedColor;
    }
}
