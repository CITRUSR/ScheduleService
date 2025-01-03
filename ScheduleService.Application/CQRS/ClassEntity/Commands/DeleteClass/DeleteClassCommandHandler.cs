using MediatR;
using ScheduleService.Application.Common.Exceptions.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Commands.DeleteClass;

public class DeleteClassCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteClassCommand, Class>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Class> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
    {
        var @class = await _unitOfWork.ClassRepository.DeleteAsync(request.Id);

        if (@class == null)
            throw new ClassNotFoundException(request.Id);

        _unitOfWork.CommitTransaction();

        return @class;
    }
}
