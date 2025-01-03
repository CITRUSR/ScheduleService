using AutoFixture;
using FluentAssertions;
using Moq;
using Npgsql;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.CreateSubject;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SubjectEntity.Commands;

public class CreateSubject
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateSubjectCommandHandler _handler;
    private readonly CreateSubjectCommand _command;

    public CreateSubject()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateSubjectCommandHandler(_mockUnitOfWork.Object);
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
        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.InsertAsync(It.IsAny<Subject>()))
            .Throws(new PostgresException("Unique", "severity", "invariantServerity", "23505"));

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<SubjectNameAlreadyExistsException>();
    }
}
