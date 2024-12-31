using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Moq;
using ScheduleService.Application.Common.Specifications.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student.GetClassesForWeekForStudent;
using ScheduleService.Application.CQRS.WeekdayEntity.Queries.GetWeekdayById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ClassEntity.Queries;

public class GetClassesForWeekForStudent
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRequestHandler<GetWeekdayByIdQuery, Weekday>> _mockWeekdayHandler;
    private readonly Fixture _fixture;
    private readonly GetClassesForWeekForStudentQuery _query;
    private readonly GetClassesForWeekForStudentQueryHandler _handler;

    public GetClassesForWeekForStudent()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockWeekdayHandler = new Mock<IRequestHandler<GetWeekdayByIdQuery, Weekday>>();
        _fixture = new Fixture();
        _query = _fixture.Create<GetClassesForWeekForStudentQuery>();
        _handler = new GetClassesForWeekForStudentQueryHandler(
            _mockUnitOfWork.Object,
            _mockWeekdayHandler.Object
        );
    }

    [Fact]
    public async Task GetClassesForWeekForStudent_ShouldBe_Success()
    {
        var weekday = _fixture.Build<Weekday>().With(x => x.Id, 4).Create();

        _fixture.Customize<Class>(x => x.With(c => c.Weekday, weekday));
        var classes = _fixture.CreateMany<Class>(5).ToList();

        _mockWeekdayHandler
            .Setup(x => x.Handle(It.IsAny<GetWeekdayByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(weekday);

        _mockUnitOfWork
            .Setup(x =>
                x.ClassRepository.GetAsync(It.IsAny<GetClassesForWeekForStudentSpecification>())
            )
            .ReturnsAsync(classes);

        var result = await _handler.Handle(_query, CancellationToken.None);

        _mockWeekdayHandler.Verify(
            x => x.Handle(It.IsAny<GetWeekdayByIdQuery>(), It.IsAny<CancellationToken>()),
            Times.Exactly(7)
        );

        _mockUnitOfWork.Verify(
            x => x.ClassRepository.GetAsync(It.IsAny<GetClassesForWeekForStudentSpecification>()),
            Times.Once()
        );

        result.Should().NotBeNull();
    }
}
