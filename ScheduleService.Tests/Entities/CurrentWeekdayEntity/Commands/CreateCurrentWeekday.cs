using AutoFixture;
using FluentAssertions;
using Moq;
using Npgsql;
using ScheduleService.Application.Common.Exceptions.CurrentWeekdayEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.CurrentWeekdayEntity.Commands.CreateCurrentWeekday;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.CurrentWeekdayEntity.Commands;

public class CreateCurrentWeekday
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateCurrentWeekdayCommandHandler _handler;
    private readonly CreateCurrentWeekdayCommand _command;

    public CreateCurrentWeekday()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateCurrentWeekdayCommandHandler(_mockUnitOfWork.Object);
        _command = _fixture.Create<CreateCurrentWeekdayCommand>();
    }

    [Fact]
    public void CreateCurrentWeekday_ShouldBe_Success()
    {
        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.InsertAsync(It.IsAny<CurrentWeekday>()))
            .ReturnsAsync(new CurrentWeekday());

        var result = _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(
            x => x.CurrentWeekdayRepository.InsertAsync(It.IsAny<CurrentWeekday>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateCurrentWeekday_ShouldBe_CurrentWeekdayAlreadyExistsException()
    {
        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.InsertAsync(It.IsAny<CurrentWeekday>()))
            .ThrowsAsync(new PostgresException("", "", "", "P0001"));

        Func<Task> act = async () => await _handler.Handle(_command, default);

        await act.Should().ThrowAsync<CurrentWeekdayAlreadyExistsException>();
    }
}
