using Mapster;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;
using ScheduleService.Application.CQRS.ClassEntity.Queries.GetClasses.Student;

namespace ScheduleService.Application.Common.Extensions;

public static class StudentClassDetailExtension
{
    public static async Task LoadTeachers(
        this List<StudentClassDetailDto> classes,
        ITeacherService teacherService
    )
    {
        List<(StudentClassDetailDto @class, Task<TeacherDto> ids)> tasks = [];

        foreach (var classDetail in classes)
        {
            foreach (var id in classDetail.TeacherIds)
            {
                tasks.Add((classDetail, teacherService.GetTeacherById(id)));
            }
        }

        await Task.WhenAll(tasks.Select(x => x.ids));

        foreach (var (@class, teacher) in tasks)
        {
            @class.Teachers.Add(teacher.Result.Adapt<TeacherViewModel>());
        }
    }
}
