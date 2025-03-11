using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Services;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.UpdateSpecialityTeacherSubject;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SpecialityTeacherSubjectEntity.Commands;

public class UpdateSpecialityTeacherSubject
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISpecialityTeacherSubjectRelatedDataChecker> _mockChecker;
    private readonly UpdateSpecialityTeacherSubjectCommandHandler _handler;

    public UpdateSpecialityTeacherSubject()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockChecker = new Mock<ISpecialityTeacherSubjectRelatedDataChecker>();
        _handler = new UpdateSpecialityTeacherSubjectCommandHandler(
            _mockUnitOfWork.Object,
            _mockChecker.Object
        );
    }

    [Fact]
    public async Task UpdateSpecialityTeacherSubject_ShouldBe_Success()
    {
        var entity = _fixture.Create<SpecialityTeacherSubject>();

        _mockChecker
            .Setup(x =>
                x.Check(
                    It.Is<SpecialityTeacherSubject>(x =>
                        x.Course == entity.Course
                        && x.SubGroup == entity.SubGroup
                        && x.SpecialityId == entity.SpecialityId
                        && x.TeacherId == entity.TeacherId
                        && x.SubjectId == entity.SubjectId
                    )
                )
            )
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(x =>
                x.SpecialityTeacherSubjectRepository.UpdateAsync(
                    It.Is<SpecialityTeacherSubject>(x =>
                        x.Course == entity.Course
                        && x.SubGroup == entity.SubGroup
                        && x.SpecialityId == entity.SpecialityId
                        && x.TeacherId == entity.TeacherId
                        && x.SubjectId == entity.SubjectId
                    )
                )
            )
            .ReturnsAsync(entity);

        var command = new UpdateSpecialityTeacherSubjectCommand(
            entity.SpecialityId,
            entity.Course,
            entity.SubGroup,
            entity.TeacherId,
            entity.SubjectId
        );

        var res = await _handler.Handle(command, default);

        res.Should().NotBeNull();

        _mockChecker.Verify(
            x =>
                x.Check(
                    It.Is<SpecialityTeacherSubject>(x =>
                        x.Course == entity.Course
                        && x.SubGroup == entity.SubGroup
                        && x.SpecialityId == entity.SpecialityId
                        && x.TeacherId == entity.TeacherId
                        && x.SubjectId == entity.SubjectId
                    )
                ),
            Times.Once()
        );

        _mockUnitOfWork.Verify(
            x =>
                x.SpecialityTeacherSubjectRepository.UpdateAsync(
                    It.Is<SpecialityTeacherSubject>(x =>
                        x.Course == entity.Course
                        && x.SubGroup == entity.SubGroup
                        && x.SpecialityId == entity.SpecialityId
                        && x.TeacherId == entity.TeacherId
                        && x.SubjectId == entity.SubjectId
                    )
                ),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());
    }
}
