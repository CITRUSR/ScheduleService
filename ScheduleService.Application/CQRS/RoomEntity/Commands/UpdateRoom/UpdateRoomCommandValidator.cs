using FluentValidation;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.UpdateRoom;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
        RuleFor(x => x.FullName).MaximumLength(128);
    }
}
