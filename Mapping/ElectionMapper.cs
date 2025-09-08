using Election.Api.Dtos;
using ElectionModel = Election.Api.Models.Election;

namespace Election.Api.Mapping;

public static class ElectionMapper
{
    private static readonly Dictionary<string, int> CategoryMap = new()
    {
        ["L"] = 1,  // Local
        ["S"] = 2,  // State
        ["T"] = 3   // Town Meeting
    };

    public static ElectionModel ToModel(CreateElectionDto dto) => new()
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        CategoryCode = dto.CategoryCode,
        CategoryId = CategoryMap[dto.CategoryCode],
        StartUtc = dto.StartUtc,
        EndUtc = dto.EndUtc,
        TimeZoneId = dto.TimeZoneId,
        Status = "Draft",
        CreatedUtc = DateTime.UtcNow,
        UpdatedUtc = DateTime.UtcNow
    };

    public static void Apply(UpdateElectionDto dto, ElectionModel e)
    {
        e.FirstName = dto.FirstName;
        e.LastName = dto.LastName;
        e.CategoryCode = dto.CategoryCode;
        e.CategoryId = CategoryMap[dto.CategoryCode];
        e.StartUtc = dto.StartUtc;
        e.EndUtc = dto.EndUtc;
        e.TimeZoneId = dto.TimeZoneId;
        e.Status = dto.Status;
        e.UpdatedUtc = DateTime.UtcNow;
    }

    public static ElectionDto ToDto(ElectionModel e) =>
        new(e.Id, e.FirstName, e.LastName, e.CategoryCode, e.CategoryId,
            e.StartUtc, e.EndUtc, e.TimeZoneId, e.Status, e.CreatedUtc, e.UpdatedUtc);
}
