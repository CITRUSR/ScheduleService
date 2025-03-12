using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Application.CQRS.RoomEntity.Commands.CreateRoom;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.RoomEntity.Commands;

public class CreateRoom
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUniqueConstraintExceptionChecker> _mockUniqueChecker;
    private readonly CreateRoomCommandHandler _handler;
    private readonly CreateRoomCommand _command;

    public CreateRoom()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUniqueChecker = new Mock<IUniqueConstraintExceptionChecker>();
        _handler = new CreateRoomCommandHandler(_mockUnitOfWork.Object, _mockUniqueChecker.Object);
        _command = _fixture.Create<CreateRoomCommand>();
    }

    [Fact]
    public async Task CreateRoom_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.RoomRepository.InsertAsync(It.IsAny<Room>()))
            .ReturnsAsync(new Room());

        var room = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(x => x.RoomRepository.InsertAsync(It.IsAny<Room>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.CommitTransaction());

        room.Id.Should().Be(0);
    }

    [Fact]
    public async Task CreateRoom_ShouldBe_RoomNameAlreadyExistsException()
    {
        var ex = new Exception("Unique ex");

        _mockUnitOfWork.Setup(x => x.RoomRepository.InsertAsync(It.IsAny<Room>())).Throws(ex);
        _mockUniqueChecker.Setup(x => x.Check<Room>(ex)).Returns("name");

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<RoomNameAlreadyExistsException>();

        _mockUnitOfWork.Verify(x => x.RoomRepository.InsertAsync(It.IsAny<Room>()), Times.Once());
        _mockUniqueChecker.Verify(x => x.Check<Room>(ex), Times.Once());
        _mockUnitOfWork.Verify(x => x.RollbackTransaction(), Times.Once());
    }
}
