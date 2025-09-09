using Election.API.Dtos;
using ElectionModel = Election.API.Models.Election;

namespace Election.API.Mapping;

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
        Name = dto.Name,
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
        e.Name = dto.Name;
        e.CategoryCode = dto.CategoryCode;
        e.CategoryId = CategoryMap[dto.CategoryCode];
        e.StartUtc = dto.StartUtc;
        e.EndUtc = dto.EndUtc;
        e.TimeZoneId = dto.TimeZoneId;
        e.Status = dto.Status;
        e.UpdatedUtc = DateTime.UtcNow;
    }

    public static ElectionDto ToDto(ElectionModel e) =>
        new(e.Id, e.Name, e.CategoryCode, e.CategoryId,
            e.StartUtc, e.EndUtc, e.TimeZoneId, e.Status, e.CreatedUtc, e.UpdatedUtc);
}
