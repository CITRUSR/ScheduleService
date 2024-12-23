using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions.ClassEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ClassEntity.Commands.DeleteClass;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ClassEntity.Commands;

public class DeleteClass
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Fixture _fixture;
    private readonly DeleteClassCommandHandler _handler;
    private readonly DeleteClassCommand _command;

    public DeleteClass()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _fixture = new Fixture();
        _handler = new DeleteClassCommandHandler(_mockUnitOfWork.Object);
        _command = _fixture.Create<DeleteClassCommand>();
    }

    [Fact]
    public async Task DeleteClass_ShouldDelete_Succes()
    {
        _mockUnitOfWork
            .Setup(u => u.ClassRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(new Class());

        var res = await _handler.Handle(_command, CancellationToken.None);

        _mockUnitOfWork.Verify(u => u.ClassRepository.DeleteAsync(_command.Id), Times.Once);

        _mockUnitOfWork.Verify(u => u.CommitTransaction(), Times.Once);

        res.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteClass_Should_ClassNotFoundException()
    {
        _mockUnitOfWork
            .Setup(u => u.ClassRepository.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync((Class?)null);

        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        await act.Should().ThrowAsync<ClassNotFoundException>();
    }
}
