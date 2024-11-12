using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.WeekdayEntity.Queries;

public class GetWeekdayById
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly GetWeekdayByIdQueryHandler _handler;
    private readonly GetWeekdayByIdQuery _query;

    public GetWeekdayById()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new GetWeekdayByIdQueryHandler(_mockUnitOfWork.Object);
        _query = _fixture.Create<GetWeekdayByIdQuery>();
    }

    [Fact]
    public async Task GetWeekdayById_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.WeekdayRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Weekday());

        var result = await _handler.Handle(_query, default);

        _mockUnitOfWork.Verify(
            x => x.WeekdayRepository.GetByIdAsync(It.IsAny<int>()),
            Times.Once()
        );

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetWeekdayById_ShouldBe_WeekdayNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.WeekdayRepository.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Weekday?)null);

        Func<Task> act = async () => await _handler.Handle(_query, default);

        await act.Should().ThrowAsync<WeekdayNotFoundException>();
    }
}
