namespace ScheduleService.Application.Contracts.UserService.Speciality.dto.Responses;

public record DeleteSpecialitiesResponse
{
    public List<SpecialityShortInfo> Specialities { get; set; }
}
