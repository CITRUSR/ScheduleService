using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.RoomEntity.Commands.DeleteRoom;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.RoomEntity.Commands;

public class DeleteRoom
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeleteRoomCommandHandler _handler;
    private readonly DeleteRoomCommand _command;

    public DeleteRoom()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new DeleteRoomCommandHandler(_mockUnitOfWork.Object);
        _command = _fixture.Create<DeleteRoomCommand>();
    }

    [Fact]
    public async Task DeleteRoom_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.RoomRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(new Room());

        await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(x => x.RoomRepository.DeleteAsync(It.IsAny<int>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());
    }

    [Fact]
    public async Task DeleteRoom_ShouldBe_RoomNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.RoomRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Room?)null);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<RoomNotFoundException>();
    }
}
