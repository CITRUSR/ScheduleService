using FluentValidation;

namespace ScheduleService.Application.CQRS.RoomEntity.Commands.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
        RuleFor(x => x.FullName).MaximumLength(128);
    }
}
