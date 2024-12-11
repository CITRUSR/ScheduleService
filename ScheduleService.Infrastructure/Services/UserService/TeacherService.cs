using Mapster;
using ScheduleService.Application.Contracts.UserService.Teacher;
using ScheduleService.Application.Contracts.UserService.Teacher.dto.responses;

namespace ScheduleService.Infrastructure.Services.UserService;

public class TeacherService(UserServiceClient.TeacherService.TeacherServiceClient client)
    : ITeacherService
{
    private readonly UserServiceClient.TeacherService.TeacherServiceClient _client = client;

    public async Task<TeacherDto> GetTeacherById(Guid id)
    {
        var grpcRequest = new UserServiceClient.GetTeacherByIdRequest { Id = id.ToString() };

        var response = await _client.GetTeacherByIdAsync(grpcRequest);

        return response.Adapt<TeacherDto>();
    }
}
