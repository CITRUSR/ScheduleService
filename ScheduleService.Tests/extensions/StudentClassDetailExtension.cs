using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;

namespace ScheduleService.Tests.extensions;

public class StudentClassDetailExtension
{
    private readonly Mock<ITeacherService> _mockTeacherService;
    private readonly Fixture _fixture;

    public StudentClassDetailExtension()
    {
        _mockTeacherService = new Mock<ITeacherService>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task LoadTeachers_ShouldBe_Success()
    {
        var classes = _fixture
            .Build<StudentClassDetailDto>()
            .Without(x => x.Teachers)
            .CreateMany(5)
            .ToList();

        foreach (var @class in classes)
        {
            foreach (var teacherId in @class.TeacherIds)
            {
                var teacher = _fixture.Build<TeacherDto>().With(x => x.Id, teacherId).Create();

                _mockTeacherService.Setup(s => s.GetTeacherById(teacherId)).ReturnsAsync(teacher);
            }
        }

        await classes.LoadTeachers(_mockTeacherService.Object);

        foreach (var @class in classes)
        {
            foreach (var teacherId in @class.TeacherIds)
            {
                _mockTeacherService.Verify(s => s.GetTeacherById(teacherId), Times.Once);
            }
        }

        foreach (var @class in classes)
        {
            @class.TeacherIds.Should().Equal(@class.Teachers.Select(x => x.Id));
        }
    }
}
