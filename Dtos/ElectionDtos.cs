using System.ComponentModel.DataAnnotations;

namespace Election.API.Dtos;

public record CreateElectionDto(                           //  For POST 
    [property: Required, StringLength(50, MinimumLength = 3)] string FirstName,
    [property: Required, StringLength(50, MinimumLength = 3)] string LastName,
    [property: Required, RegularExpression("^[LST]$", ErrorMessage = "CategoryCode must be L, S, or T.")] string CategoryCode,
    [property: Required] DateTime StartUtc,
    [property: Required] DateTime EndUtc,
    [property: Required, StringLength(64)] string TimeZoneId
);

public class UpdateElectionDto                             // For PUT
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string FirstName { get; set; } = default!;
    
    [Required, StringLength(100, MinimumLength = 3)]
    public string LastName { get; set; } = default!;

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
    string FirstName,
    string LastName,
    string CategoryCode,
    int CategoryId,
    DateTime StartUtc,
    DateTime EndUtc,
    string TimeZoneId,
    string Status,
    DateTime CreatedUtc,
    DateTime UpdatedUtc
);
