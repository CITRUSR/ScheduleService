using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Exceptions.SpecialityTeacherSubjectEntity;
using ScheduleService.Application.Contracts;
using ScheduleService.Application.CQRS.SpecialityTeacherSubjectEntity.Queries.GetSpecialityTeacherSubjectById;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.Entities.SpecialityTeacherSubjectEntity.Queries;

public class GetSpecialityTeacherSubjectById
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly IFixture _fixture;
    private readonly GetSpecialityTeacherSubjectByIdQueryHandler _handler;

    public GetSpecialityTeacherSubjectById()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _fixture = new Fixture();
        _handler = new GetSpecialityTeacherSubjectByIdQueryHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetSpecialityTeacherSubjectById_ShouldBe_Success()
    {
        var specialityTeacherSubject = _fixture.Create<SpecialityTeacherSubject>();

        _mockUnitOfWork
            .Setup(x =>
                x.SpecialityTeacherSubjectRepository.GetByIdAsync(
                    specialityTeacherSubject.SpecialityId,
                    specialityTeacherSubject.Course,
                    specialityTeacherSubject.SubGroup
                )
            )
            .ReturnsAsync(specialityTeacherSubject);

        var query = new GetSpecialityTeacherSubjectByIdQuery(
            specialityTeacherSubject.SpecialityId,
            specialityTeacherSubject.Course,
            specialityTeacherSubject.SubGroup
        );

        var entity = await _handler.Handle(query, default);

        _mockUnitOfWork.Verify(
            x =>
                x.SpecialityTeacherSubjectRepository.GetByIdAsync(
                    specialityTeacherSubject.SpecialityId,
                    specialityTeacherSubject.Course,
                    specialityTeacherSubject.SubGroup
                ),
            Times.Once()
        );

        entity.Should().NotBeNull();
        entity.SpecialityId = specialityTeacherSubject.SpecialityId;
        entity.Course = specialityTeacherSubject.Course;
        entity.SubGroup = specialityTeacherSubject.SubGroup;
    }

    [Fact]
    public async Task GetSpecialityTeacherSubjectById_ShouldBe_SpecialityTeacherSubjectNotFoundException()
    {
        _mockUnitOfWork
            .Setup(x =>
                x.SpecialityTeacherSubjectRepository.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )
            )
            .ReturnsAsync((SpecialityTeacherSubject?)null);

        var query = new GetSpecialityTeacherSubjectByIdQuery(1, 2, 3);

        Func<Task> act = async () => await _handler.Handle(query, default);

        await act.Should().ThrowAsync<SpecialityTeacherSubjectNotFoundException>();
    }
}
