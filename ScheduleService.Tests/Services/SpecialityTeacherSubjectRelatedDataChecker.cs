using AutoFixture;
using MediatR;
using Moq;
using ScheduleService.Application.Contracts.UserService.Speciality;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.CQRS.SubjectEntity.Queries.GetSubjectById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Services;

public class SpecialityTeacherSubjectRelatedDataChecker
{
    private readonly IFixture _fixture;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ISpecialityService> _mockSpecialityService;
    private readonly Mock<ITeacherService> _mockTeacherService;

    private readonly Application.Common.Services.SpecialityTeacherSubjectRelatedDataChecker _checker;

    public SpecialityTeacherSubjectRelatedDataChecker()
    {
        _fixture = new Fixture();
        _mockMediator = new Mock<IMediator>();
        _mockSpecialityService = new Mock<ISpecialityService>();
        _mockTeacherService = new Mock<ITeacherService>();
        _checker = new Application.Common.Services.SpecialityTeacherSubjectRelatedDataChecker(
            _mockMediator.Object,
            _mockTeacherService.Object,
            _mockSpecialityService.Object
        );
    }

    [Fact]
    public async Task Check_ShouldBe_Success()
    {
        var entity = _fixture.Create<SpecialityTeacherSubject>();

        await _checker.Check(entity);

        _mockMediator.Verify(
            x =>
                x.Send(
                    It.Is<GetSubjectByIdQuery>(x => x.Id == entity.SubjectId),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once()
        );

        _mockSpecialityService.Verify(x => x.GetSpecialityById(entity.SpecialityId), Times.Once());
        _mockTeacherService.Verify(x => x.GetTeacherById(entity.TeacherId), Times.Once());
    }
}
