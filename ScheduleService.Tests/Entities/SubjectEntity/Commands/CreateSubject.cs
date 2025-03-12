using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.Contracts.Helpers;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SubjectEntity.Commands;

public class CreateSubject
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUniqueConstraintExceptionChecker> _mockUniqueChecker;
    private readonly CreateSubjectCommandHandler _handler;
    private readonly CreateSubjectCommand _command;

    public CreateSubject()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUniqueChecker = new Mock<IUniqueConstraintExceptionChecker>();
        _handler = new CreateSubjectCommandHandler(
            _mockUnitOfWork.Object,
            _mockUniqueChecker.Object
        );
        _command = _fixture.Create<CreateSubjectCommand>();
    }

    [Fact]
    public async Task CreateSubject_ShouldBe_Success()
    {
        var subject = _fixture.Create<Subject>();

        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.InsertAsync(It.IsAny<Subject>()))
            .ReturnsAsync(subject);

        var createdSubject = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(
            x => x.SubjectRepository.InsertAsync(It.IsAny<Subject>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        createdSubject.Id.Should().Be(subject.Id);
    }

    [Fact]
    public async Task CreateSubject_ShouldBe_SubjectNameAlreadyExistsException()
    {
        var ex = new Exception("Unique ex");

        _mockUnitOfWork.Setup(x => x.SubjectRepository.InsertAsync(It.IsAny<Subject>())).Throws(ex);

        _mockUniqueChecker.Setup(x => x.Check<Subject>(ex)).Returns("name");

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<SubjectNameAlreadyExistsException>();

        _mockUnitOfWork.Verify(
            x => x.SubjectRepository.InsertAsync(It.IsAny<Subject>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.RollbackTransaction(), Times.Once());

        _mockUniqueChecker.Verify(x => x.Check<Subject>(ex), Times.Once());
    }
}
