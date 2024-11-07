using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.RoomEntity.Queries.GetRoomById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.RoomEntity.Queries;

public class GetRoomById
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly GetRoomByIdQueryHandler _handler;
    private readonly GetRoomByIdQuery _query;

    public GetRoomById()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new GetRoomByIdQueryHandler(_mockUnitOfWork.Object);
        _query = _fixture.Create<GetRoomByIdQuery>();
    }

    [Fact]
    public async Task GetRoomById_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.RoomRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Room());

        var room = await _handler.Handle(_query, default);

        _mockUnitOfWork.Verify(x => x.RoomRepository.GetByIdAsync(It.IsAny<int>()), Times.Once());

        room.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRoomById_ShouldBe_RoomNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.RoomRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Room?)null);

        Func<Task> act = async () => await _handler.Handle(_query, default);

        await act.Should().ThrowAsync<RoomNotFoundException>();
    }
}
