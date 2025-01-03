using AutoFixture;
using FluentAssertions;
using Moq;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.RoomEntity.Commands.UpdateRoom;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.RoomEntity.Commands;

public class UpdateRoom
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UpdateRoomCommandHandler _handler;
    private readonly UpdateRoomCommand _command;

    public UpdateRoom()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateRoomCommandHandler(_mockUnitOfWork.Object);
        _command = _fixture.Build<UpdateRoomCommand>().Create();
    }

    [Fact]
    public async Task UpdateRoom_ShouldBe_Success()
    {
        var room = _fixture.Create<Room>();

        _mockUnitOfWork
            .Setup(x => x.RoomRepository.UpdateAsync(It.IsAny<Room>()))
            .ReturnsAsync(room);

        var result = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(x => x.RoomRepository.UpdateAsync(It.IsAny<Room>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        result.Id.Should().Be(room.Id);
    }

    [Fact]
    public async Task UpdateRoom_ShouldBe_RoomNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.RoomRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Room?)null);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<RoomNotFoundException>();
    }

    [Fact]
    public async Task UpdateRoom_ShouldBe_RoomNameAlreadyExistsException()
    {
        _mockUnitOfWork
            .Setup(x => x.RoomRepository.UpdateAsync(It.IsAny<Room>()))
            .ReturnsAsync(new Room());

        _mockUnitOfWork
            .Setup(x => x.RoomRepository.UpdateAsync(It.IsAny<Room>()))
            .Throws(new PostgresException("Unique", "severity", "invariantServerity", "23505"));

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<RoomNameAlreadyExistsException>();
    }
}
