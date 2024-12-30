using AutoFixture;
using FluentAssertions;
using MediatR;
using Moq;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.GetClassOnCurrentDateForStudents;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassOnCurrentDateForStudents;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ClassEntity.Queries;

public class GetClassesOnCurrentDateForStudent
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRequestHandler<GetWeekdayByIdQuery, Weekday>> _weekdayHandler;
    private readonly Fixture _fixture;
    private readonly GetClassesOnCurrentDateForStudentQuery _query;
    private readonly GetClassesOnCurrentDateForStudentQueryHandler _handler;

    public GetClassesOnCurrentDateForStudent()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _weekdayHandler = new Mock<IRequestHandler<GetWeekdayByIdQuery, Weekday>>();
        _fixture = new Fixture();
        _query = _fixture.Create<GetClassesOnCurrentDateForStudentQuery>();
        _handler = new GetClassesOnCurrentDateForStudentQueryHandler(
            _mockUnitOfWork.Object,
            _weekdayHandler.Object
        );
    }

    [Fact]
    public async Task GetClassesOnCurrentDateForStudent_ShouldBe_Success()
    {
        var classes = _fixture.CreateMany<Class>(5).ToList();

        _mockUnitOfWork
            .Setup(x => x.ClassRepository.GetAsync(It.IsAny<IClassSpecification>()))
            .ReturnsAsync(classes);

        _weekdayHandler
            .Setup(x => x.Handle(It.IsAny<GetWeekdayByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Weekday());

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Classes.Select(x => x.Classes.Select((d, index) => d.Order.Should().Be(index + 1)));

        _weekdayHandler.Verify(
            x => x.Handle(It.IsAny<GetWeekdayByIdQuery>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(
            x => x.ClassRepository.GetAsync(It.IsAny<IClassSpecification>()),
            Times.Once()
        );

        result.Should().NotBeNull();
    }
}
