using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdays;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.WeekdayEntity.Queries;

public class GetWeekdays
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public GetWeekdays()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task GetWeekdays_ShouldBe_Success()
    {
        var handler = new GetWeekdaysQueryHandler(_mockUnitOfWork.Object);
        var query = _fixture.Create<GetWeekdaysQuery>();

        var weekdays = _fixture.CreateMany<Weekday>(7);

        _mockUnitOfWork.Setup(x => x.WeekdayRepository.GetAllAsync()).ReturnsAsync([.. weekdays]);

        var result = await handler.Handle(query, default);

        _mockUnitOfWork.Verify(x => x.WeekdayRepository.GetAllAsync(), Times.Once());

        result.Should().HaveCount(7);
    }
}
