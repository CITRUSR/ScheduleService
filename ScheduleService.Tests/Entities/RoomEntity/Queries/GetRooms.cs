using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Models;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.RoomEntity.Queries.GetRooms;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.RoomEntity.Queries;

public class GetRooms
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public GetRooms()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task GetRooms_ShouldBe_Success()
    {
        var rooms = _fixture.CreateMany<Room>(10);

        _mockUnitOfWork
            .Setup(x =>
                x.RoomRepository.GetAsync(It.IsAny<RoomFilter>(), It.IsAny<PaginationParameters>())
            )
            .ReturnsAsync(new PagedList<Room>([.. rooms], 1, 1, 1));

        var handler = new GetRoomsQueryHandler(_mockUnitOfWork.Object);
        var query = _fixture.Create<GetRoomsQuery>();

        var result = await handler.Handle(query, default);

        _mockUnitOfWork.Verify(
            x =>
                x.RoomRepository.GetAsync(It.IsAny<RoomFilter>(), It.IsAny<PaginationParameters>()),
            Times.Once()
        );

        result.Items.Should().HaveCount(10);
    }
}
