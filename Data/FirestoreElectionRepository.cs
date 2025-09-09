using Google.Cloud.Firestore;
using ElectionModel = Election.API.Models.Election;

namespace Election.API.Data;

public class FirestoreElectionRepository : IElectionRepository
{
    private readonly CollectionReference _col;

    public FirestoreElectionRepository(FirestoreDb db)
    {
        _col = db.Collection("elections");
    }

    public async Task<string> CreateAsync(ElectionModel election, CancellationToken ct = default)
    {
        // create doc, Firestore auto-generates id
        var docRef = await _col.AddAsync(election, ct);
        var id = docRef.Id;

        // persist Id field back to the same document for convenience
        await docRef.UpdateAsync(new Dictionary<string, object> { ["Id"] = id });
        return id;
    }

    public async Task<ElectionModel?> GetAsync(string id, CancellationToken ct = default)
    {
        var snap = await _col.Document(id).GetSnapshotAsync(ct);
        return snap.Exists ? snap.ConvertTo<ElectionModel>() : null;
    }

    public async Task<IReadOnlyList<ElectionModel>> ListAsync(int limit = 50, string? status = null, CancellationToken ct = default)
    {
        limit = Math.Clamp(limit, 1, 200);

        Query q = _col.OrderByDescending("CreatedUtc").Limit(limit);
        if (!string.IsNullOrWhiteSpace(status))
        {
            // filter by status if requested (Draft|Scheduled|Open|Closed)
            q = q.WhereEqualTo("Status", status);
            // note: Firestore may prompt you to create a composite index for (Status + CreatedUtc).
        }

        
        var snaps = await q.GetSnapshotAsync(ct);
        return snaps.Documents.Select(d => d.ConvertTo<ElectionModel>()).ToList();
    }

    public async Task<bool> UpdateAsync(ElectionModel election, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(election.Id)) return false;

        var doc = _col.Document(election.Id);
        var snap = await doc.GetSnapshotAsync(ct);
        if (!snap.Exists) return false;

        election.UpdatedUtc = DateTime.UtcNow;
        await doc.SetAsync(election, cancellationToken: ct);
        return true;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct = default)
    {
        var doc = _col.Document(id);
        var snap = await doc.GetSnapshotAsync(ct);
        if (!snap.Exists) return false;

        await doc.DeleteAsync();
        return true;
    }
}
