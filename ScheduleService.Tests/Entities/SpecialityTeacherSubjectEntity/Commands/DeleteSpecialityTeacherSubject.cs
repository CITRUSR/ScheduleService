using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Commands.DeleteSpecialityTeacherSubject;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SpecialityTeacherSubjectEntity.Commands;

public class DeleteSpecialityTeacherSubject
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeleteSpecialityTeacherSubjectCommandHandler _handler;

    public DeleteSpecialityTeacherSubject()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new DeleteSpecialityTeacherSubjectCommandHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task DeleteSpecialityTeacherSubject_ShouldBe_Success()
    {
        var entity = _fixture.Create<SpecialityTeacherSubject>();

        _mockUnitOfWork
            .Setup(x =>
                x.SpecialityTeacherSubjectRepository.DeleteAsync(
                    entity.SpecialityId,
                    entity.Course,
                    entity.SubGroup
                )
            )
            .ReturnsAsync(entity);

        var command = new DeleteSpecialityTeacherSubjectCommand(
            entity.SpecialityId,
            entity.Course,
            entity.SubGroup
        );

        var res = await _handler.Handle(command, default);

        res.Should().NotBeNull();

        _mockUnitOfWork.Verify(
            x =>
                x.SpecialityTeacherSubjectRepository.DeleteAsync(
                    entity.SpecialityId,
                    entity.Course,
                    entity.SubGroup
                ),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());
    }

    [Fact]
    public async Task DeleteSpecialityTeacherSubject_ShouldBe_SpecialitySubjectNotFoundException()
    {
        var entity = _fixture.Create<SpecialityTeacherSubject>();

        _mockUnitOfWork
            .Setup(x =>
                x.SpecialityTeacherSubjectRepository.DeleteAsync(
                    entity.SpecialityId,
                    entity.Course,
                    entity.SubGroup
                )
            )
            .ReturnsAsync((SpecialityTeacherSubject?)null);

        var command = new DeleteSpecialityTeacherSubjectCommand(
            entity.SpecialityId,
            entity.Course,
            entity.SubGroup
        );

        Func<Task> act = async () => await _handler.Handle(command, default);

        await act.Should().ThrowAsync<SpecialityTeacherSubjectNotFoundException>();

        _mockUnitOfWork.Verify(
            x =>
                x.SpecialityTeacherSubjectRepository.DeleteAsync(
                    entity.SpecialityId,
                    entity.Course,
                    entity.SubGroup
                ),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Never());
    }
}
