using MediatR;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Application.CQRS.ColorEntity.Commands.UpdateColor;

public class UpdateColorCommandHandler(
    IUnitOfWork unitOfWork,
    IUniqueConstraintExceptionChecker uniqueChecker
) : IRequestHandler<UpdateColorCommand, Color>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUniqueConstraintExceptionChecker _uniqueChecker = uniqueChecker;

    public async Task<Color> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
    {
        var newColor = new Color() { Id = request.Id, Name = request.Name };

        Color updatedColor;

        try
        {
            updatedColor = await _unitOfWork.ColorRepository.UpdateAsync(newColor);

            if (updatedColor == null)
            {
                throw new ColorNotFoundException(request.Id);
            }

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

        return updatedColor;
    }
}
