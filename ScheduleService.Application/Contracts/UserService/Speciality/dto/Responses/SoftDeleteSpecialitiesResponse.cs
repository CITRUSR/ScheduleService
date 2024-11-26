namespace ScheduleService.Application.Contracts.UserService.Speciality.dto.Responses;

public record SoftDeleteSpecialitiesResponse
{
    public List<SpecialityShortInfo> Specialities { get; set; }
}
