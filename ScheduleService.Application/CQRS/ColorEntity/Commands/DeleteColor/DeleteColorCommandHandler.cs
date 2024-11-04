using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.DeleteColor;

public class DeleteColorCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteColorCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.ColorRepository.GetByIdAsync(request.Id);

        if (color == null)
        {
            throw new ColorNotFoundException(request.Id);
        }

        await _unitOfWork.ColorRepository.DeleteAsync(request.Id);

        _unitOfWork.CommitTransaction();

        return Unit.Value;
    }
}
