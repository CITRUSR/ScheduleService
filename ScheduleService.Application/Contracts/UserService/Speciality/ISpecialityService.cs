using ScheduleService.Application.Contracts.UserService.Speciality.dto.Responses;

namespace ScheduleService.Application.Contracts.UserService.Speciality;

public interface ISpecialityService
{
    Task<SpecialityDto> GetSpecialityById(int id);
}
