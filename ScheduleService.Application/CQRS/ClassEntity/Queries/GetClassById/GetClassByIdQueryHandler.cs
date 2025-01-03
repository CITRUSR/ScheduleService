using MediatR;
using ScheduleService.Application.Common.Exceptions.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ClassEntity.Queries.GetClassById;

public class GetClassByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetClassByIdQuery, Class>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Class> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
    {
        var @class = await _unitOfWork.ClassRepository.GetByIdAsync(request.Id);

        if (@class == null)
            throw new ClassNotFoundException(request.Id);

        return @class;
    }
}
