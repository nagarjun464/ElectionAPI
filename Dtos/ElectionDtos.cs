using System.ComponentModel.DataAnnotations;

namespace Election.API.Dtos;

public record CreateElectionDto                          //  For POST 
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = default!;

    [Required, RegularExpression("^[LST]$")]
    public string CategoryCode { get; set; } = default!;

    [Required] public DateTime StartUtc { get; set; }
    [Required] public DateTime EndUtc { get; set; }

    [Required, StringLength(64)]
    public string TimeZoneId { get; set; } = default!;
}

public class UpdateElectionDto                             // For PUT
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = default!;

    [Required, RegularExpression("^[LST]$")]
    public string CategoryCode { get; set; } = default!;

    [Required] public DateTime StartUtc { get; set; }
    [Required] public DateTime EndUtc { get; set; }

    [Required, StringLength(64)]
    public string TimeZoneId { get; set; } = default!;

    [Required, RegularExpression("^(Draft|Scheduled|Open|Closed)$")]
    public string Status { get; set; } = "Draft";
}

public record ElectionDto(
    string Id,
    string Name,
    string CategoryCode,
    int CategoryId,
    DateTime StartUtc,
    DateTime EndUtc,
    string TimeZoneId,
    string Status,
    DateTime CreatedUtc,
    DateTime UpdatedUtc
);
