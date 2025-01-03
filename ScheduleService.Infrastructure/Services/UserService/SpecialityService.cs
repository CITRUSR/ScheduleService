using Mapster;
using ScheduleService.Application.Contracts.UserService.Speciality;
using ScheduleService.Application.Contracts.UserService.Speciality.dto.Responses;

namespace ScheduleService.Infrastructure.Services.UserService;

public class SpecialityService(UserServiceClient.SpecialityService.SpecialityServiceClient client)
    : ISpecialityService
{
    private readonly UserServiceClient.SpecialityService.SpecialityServiceClient _client = client;

    public async Task<SpecialityDto> GetSpecialityById(int id)
    {
        var grpcRequest = new UserServiceClient.GetSpecialityByIdRequest { Id = id };

        var result = await _client.GetSpecialityByIdAsync(grpcRequest);

        return result.Adapt<SpecialityDto>();
    }
}
