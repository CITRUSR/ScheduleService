using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.ColorEntity.Queries.GetColors;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.ColorEntity.Queries;

public class GetColors
{
    private readonly Fixture _fixture;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public GetColors()
    {
        _fixture = new Fixture();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task GetColors_ShouldBe_Success()
    {
        var colors = _fixture.CreateMany<Color>(5);

        _mockUnitOfWork.Setup(x => x.ColorRepository.GetAllAsync()).ReturnsAsync([.. colors]);

        var query = _fixture.Create<GetColorsQuery>();

        var handler = new GetColorsQueryHandler(_mockUnitOfWork.Object);

        var result = await handler.Handle(query, default);

        _mockUnitOfWork.Verify(x => x.ColorRepository.GetAllAsync(), Times.Once());

        result.Should().HaveCount(5);
    }
}
