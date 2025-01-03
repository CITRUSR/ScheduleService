using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SubjectEntity.Commands.DeleteSubject;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SubjectEntity.Commands;

public class DeleteSubject
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeleteSubjectCommandHandler _handler;
    private readonly DeleteSubjectCommand _command;

    public DeleteSubject()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new DeleteSubjectCommandHandler(_mockUnitOfWork.Object);
        _command = _fixture.Create<DeleteSubjectCommand>();
    }

    [Fact]
    public async Task DeleteSubject_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(new Subject());

        var res = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(x => x.SubjectRepository.DeleteAsync(It.IsAny<int>()), Times.Once);

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once);

        res.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteSubject_ShouldBe_SubjectNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x => x.SubjectRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync((Subject?)null);

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<SubjectNotFoundException>();
    }
}
