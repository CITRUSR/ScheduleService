using MediatR;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.CreateColor;

public class CreateColorCommandHandler(
    IUnitOfWork unitOfWork,
    IUniqueConstraintExceptionChecker uniqueChecker
) : IRequestHandler<CreateColorCommand, Color>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUniqueConstraintExceptionChecker _uniqueChecker = uniqueChecker;

    public async Task<Color> Handle(CreateColorCommand request, CancellationToken cancellationToken)
    {
        Color color = new() { Name = request.Name };

        Color insertedColor;

        try
        {
            insertedColor = await _unitOfWork.ColorRepository.InsertAsync(color);

            _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            var propName = _uniqueChecker.Check<Color>(ex);

            if (propName != null && propName.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                throw new ColorNameAlreadyExistsException(request.Name);
            }

            _unitOfWork.RollbackTransaction();
            throw;
        }

        return insertedColor;
    }
}
