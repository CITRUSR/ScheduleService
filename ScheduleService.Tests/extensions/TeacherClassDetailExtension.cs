using AutoFixture;
using FluentAssertions;
using Moq;
using ScheduleService.Application.Common.Extensions;
using ScheduleService.Application.Contracts.UserService.Group;
using ScheduleService.Application.Contracts.UserService.Group.Dto.Responses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Teacher;
using ScheduleService.Domain.Entities;

namespace ScheduleService.Tests.extensions;

public class TeacherClassDetailExtension
{
    private readonly Fixture _fixture;
    private readonly Mock<IGroupService> _groupService;

    public TeacherClassDetailExtension()
    {
        _fixture = new Fixture();
        _groupService = new Mock<IGroupService>();
    }

    [Fact]
    public async Task LoadGroups_ShouldBe_Success()
    {
        var classes = _fixture
            .Build<TeacherClassDetailDto>()
            .Without(x => x.Group)
            .CreateMany(5)
            .ToList();

        foreach (var @class in classes)
        {
            _groupService
                .Setup(s => s.GetGroupById(@class.GroupId))
                .ReturnsAsync(_fixture.Build<GroupDto>().With(x => x.Id, @class.GroupId).Create());
        }

        await classes.LoadGroups(_groupService.Object);

        foreach (var @class in classes)
        {
            _groupService.Verify(s => s.GetGroupById(@class.GroupId), Times.Once);
        }

        classes.Select(x => x.GroupId).Should().Equal(classes.Select(x => x.Group.Id));
    }
}
