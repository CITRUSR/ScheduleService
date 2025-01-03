using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.DeleteColor;

public class DeleteColorCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteColorCommand, Color>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Color> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.ColorRepository.DeleteAsync(request.Id);

        if (color == null)
        {
            throw new ColorNotFoundException(request.Id);
        }

        _unitOfWork.CommitTransaction();

        return color;
    }
}
