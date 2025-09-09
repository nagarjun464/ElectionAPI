using Google.Cloud.Firestore;

namespace Election.API.Models;

[FirestoreData]                       // Tells the SDK this class maps to a Firestore document
public class Election
{
    [FirestoreDocumentId]             // Auto-binds the Firestore document Id to this property
    public string Id { get; set; } = default!;

    [FirestoreProperty] public string Name { get; set; } = default!;
    // L = Local, S = State, T = Town Meeting (we’ll map to an int in the mapper)
    [FirestoreProperty] public string CategoryCode { get; set; } = default!;
    [FirestoreProperty] public int CategoryId { get; set; }

    [FirestoreProperty] public DateTime StartUtc { get; set; }
    [FirestoreProperty] public DateTime EndUtc { get; set; }
    [FirestoreProperty] public string TimeZoneId { get; set; } = "America/Chicago";

    // Draft, Scheduled, Open, Closed
    [FirestoreProperty] public string Status { get; set; } = "Draft";

    [FirestoreProperty] public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    [FirestoreProperty] public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
}
