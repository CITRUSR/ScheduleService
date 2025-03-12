using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.UpdateSubject;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SubjectEntity.Commands;

public class UpdateSubject
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUniqueConstraintExceptionChecker> _mockUniqueChecker;
    private readonly UpdateSubjectCommandHandler _handler;
    private readonly UpdateSubjectCommand _command;

    public UpdateSubject()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUniqueChecker = new Mock<IUniqueConstraintExceptionChecker>();
        _handler = new UpdateSubjectCommandHandler(
            _mockUnitOfWork.Object,
            _mockUniqueChecker.Object
        );
        _command = _fixture.Create<UpdateSubjectCommand>();
    }

    [Fact]
    public async Task UpdateSubject_ShouldBe_Success()
    {
        var subject = _fixture.Create<Subject>();

        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.UpdateAsync(It.IsAny<Subject>()))
            .ReturnsAsync(subject);

        var result = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(
            x => x.SubjectRepository.UpdateAsync(It.IsAny<Subject>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        result.Id.Should().Be(subject.Id);
    }

    [Fact]
    public async Task UpdateSubject_ShouldBe_SubjectNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.UpdateAsync(It.IsAny<Subject>()))
            .ReturnsAsync((Subject?)null);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<SubjectNotFoundException>();
    }

    [Fact]
    public async Task UpdateSubject_ShouldBe_SubjectNameAlreadyExistsException()
    {
        var ex = new Exception("Unique ex");

        _mockUnitOfWork.Setup(x => x.SubjectRepository.UpdateAsync(It.IsAny<Subject>())).Throws(ex);

        _mockUniqueChecker.Setup(x => x.Check<Subject>(ex)).Returns("name");

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<SubjectNameAlreadyExistsException>();

        _mockUnitOfWork.Verify(
            x => x.SubjectRepository.UpdateAsync(It.IsAny<Subject>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.RollbackTransaction(), Times.Once());

        _mockUniqueChecker.Verify(x => x.Check<Subject>(ex), Times.Once());
    }
}
