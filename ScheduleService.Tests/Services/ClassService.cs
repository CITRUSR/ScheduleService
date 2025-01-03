using AutoFixture;
using FluentAssertions;
using MediatR;
using Moq;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdays;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Services;

public class ClassService
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMediator> _mockMediator;
    private readonly IClassService _classService;
    private readonly Fixture _fixture;

    public ClassService()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMediator = new Mock<IMediator>();
        _classService = new Application.Common.Services.ClassService(
            _mockUnitOfWork.Object,
            _mockMediator.Object
        );
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetClassesForDay_ShouldBe_Success()
    {
        var weekday = _fixture.Create<Weekday>();

        _mockMediator
            .Setup(x => x.Send(It.IsAny<GetWeekdayByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(weekday);

        SetupUnitOfWork();

        var result = await _classService.GetClassesForDay<ClassDetailBase>(
            It.IsAny<IClassSpecification>(),
            It.IsAny<int>()
        );

        _mockMediator.Verify(
            x => x.Send(It.IsAny<GetWeekdayByIdQuery>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(
            x => x.ClassRepository.GetAsync(It.IsAny<IClassSpecification>()),
            Times.Once()
        );

        result.weekday.Should().BeEquivalentTo(weekday);
    }

    [Fact]
    public async Task GetClassesForWeek_ShouldBe_Success()
    {
        _fixture.Customize<Weekday>(x => x.With(c => c.Id, 2));
        var weekdays = _fixture.CreateMany<Weekday>(5).ToList();

        SetupUnitOfWork();

        var result = await _classService.GetClassesForWeek<ClassDetailBase>(
            It.IsAny<IClassSpecification>()
        );

        _mockUnitOfWork.Verify(
            x => x.ClassRepository.GetAsync(It.IsAny<IClassSpecification>()),
            Times.Once()
        );

        result.Should().NotBeNull();
    }

    private List<Class> SetupUnitOfWork()
    {
        var classes = _fixture.CreateMany<Class>(5).ToList();

        _mockUnitOfWork
            .Setup(x => x.ClassRepository.GetAsync(It.IsAny<IClassSpecification>()))
            .ReturnsAsync(classes);

        return classes;
    }
}
