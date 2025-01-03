using AutoFixture;
using FluentAssertions;
using MediatR;
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
    private readonly Mock<IPublisher> _publisher;
    private readonly CreateCurrentWeekdayCommandHandler _handler;
    private readonly CreateCurrentWeekdayCommand _command;

    public CreateCurrentWeekday()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _publisher = new Mock<IPublisher>();
        _handler = new CreateCurrentWeekdayCommandHandler(
            _mockUnitOfWork.Object,
            _publisher.Object
        );
        _command = _fixture.Create<CreateCurrentWeekdayCommand>();
    }

    [Fact]
    public async Task CreateCurrentWeekday_ShouldBe_Success()
    {
        var currentWeekday = _fixture.Create<CurrentWeekday>();

        _mockUnitOfWork
            .Setup(x => x.CurrentWeekdayRepository.InsertAsync(It.IsAny<CurrentWeekday>()))
            .ReturnsAsync(currentWeekday);

        var result = await _handler.Handle(_command, default);

        _mockUnitOfWork.Verify(
            x => x.CurrentWeekdayRepository.InsertAsync(It.IsAny<CurrentWeekday>()),
            Times.Once()
        );

        _mockUnitOfWork.Verify(x => x.CommitTransaction(), Times.Once());
        _publisher.Verify(
            x => x.Publish(It.IsAny<CreateCurrentWeekdayEvent>(), It.IsAny<CancellationToken>()),
            Times.Once()
        );

        result.Id.Should().Be(currentWeekday.Id);
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
