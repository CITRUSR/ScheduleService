using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Queries.GetCurrentWeekday;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.CurrentWeekdayEntity.Queries;

public class GetCurrentWeekday
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly GetCurrentWeekdayQueryHandler _handler;
    private readonly GetCurrentWeekdayQuery _query;

    public GetCurrentWeekday()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new GetCurrentWeekdayQueryHandler(_mockUnitOfWork.Object);
        _query = _fixture.Create<GetCurrentWeekdayQuery>();
    }

    [Fact]
    public async Task GetCurrentWeekday_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.GetAsync())
            .ReturnsAsync(new CurrentWeekday());

        var result = await _handler.Handle(_query, CancellationToken.None);

        _mockUnitOfWork.Verify(x => x.CurrentWeekdayRepository.GetAsync(), Times.Once());

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCurrentWeekday_ShouldBe_CurrentWeekdayNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.GetAsync())
            .ReturnsAsync((CurrentWeekday?)null);

        Func<Task> act = async () => await _handler.Handle(_query, CancellationToken.None);

        await act.Should().ThrowAsync<CurrentWeekdayNotFoundException>();
    }
}
