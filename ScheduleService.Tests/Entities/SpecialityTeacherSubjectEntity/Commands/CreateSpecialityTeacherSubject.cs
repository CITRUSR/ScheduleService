using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.CreateSpecialityTeacherSubject;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SpecialityTeacherSubjectEntity.Commands;

public class CreateSpecialityTeacherSubject
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISpecialityTeacherSubjectRelatedDataChecker> _mockChecker;
    private readonly CreateSpecialityTeacherSubjectCommandHandler _handler;

    public CreateSpecialityTeacherSubject()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockChecker = new Mock<ISpecialityTeacherSubjectRelatedDataChecker>();
        _handler = new CreateSpecialityTeacherSubjectCommandHandler(
            _mockUnitOfWork.Object,
            _mockChecker.Object
        );
    }

    [Fact]
    public async Task CreateSpecialityTeacherSubject_ShouldBe_Success()
    {
        var entity = _fixture.Create<SpecialityTeacherSubject>();

        _mockUnitOfWork
            .Setup(x => x.SpecialityTeacherSubjectRepository.InsertAsync(entity))
            .ReturnsAsync(entity);

        var command = new CreateSpecialityTeacherSubjectCommand(
            entity.SpecialityId,
            entity.Course,
            entity.SubGroup,
            entity.TeacherId,
            entity.SubjectId
        );

        var res = await _handler.Handle(command, default);

        _mockChecker.Verify(
            x =>
                x.Check(
                    It.Is<SpecialityTeacherSubject>(x =>
                        x.Course == entity.Course
                        && x.SubGroup == entity.SubGroup
                        && x.SubjectId == entity.SubjectId
                        && x.TeacherId == entity.TeacherId
                        && x.SpecialityId == entity.SpecialityId
                    )
                ),
            Times.Once()
        );

        _mockUnitOfWork.Verify(
            x =>
                x.SpecialityTeacherSubjectRepository.InsertAsync(
                    It.Is<SpecialityTeacherSubject>(x =>
                        x.Course == entity.Course
                        && x.SubGroup == entity.SubGroup
                        && x.SpecialityId == entity.SpecialityId
                        && x.SubjectId == entity.SubjectId
                        && x.TeacherId == entity.TeacherId
                    )
                ),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        res.Should().NotBeNull();
    }
}
