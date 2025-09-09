using System.Collections.Concurrent;
using ElectionModel = Election.API.Models.Election;

namespace Election.API.Data;

public class InMemoryElectionRepository : IElectionRepository
{
    private readonly ConcurrentDictionary<string, ElectionModel> _store = new();

    public Task<string> CreateAsync(ElectionModel e, CancellationToken ct = default)
    {
        e.Id ??= Guid.NewGuid().ToString("n");
        var now = DateTime.UtcNow;
        e.CreatedUtc = now;
        e.UpdatedUtc = now;
        _store[e.Id] = e;
        return Task.FromResult(e.Id);
    }

    public Task<ElectionModel?> GetAsync(string id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id, out var e) ? e : null);

    public Task<IReadOnlyList<ElectionModel>> ListAsync(int limit = 50, string? status = null, CancellationToken ct = default)
    {
        var q = _store.Values.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(status))
            q = q.Where(x => string.Equals(x.Status, status, StringComparison.OrdinalIgnoreCase));
        var res = q.OrderByDescending(x => x.CreatedUtc)
                   .Take(Math.Clamp(limit, 1, 200))
                   .ToList();
        return Task.FromResult<IReadOnlyList<ElectionModel>>(res);
    }

    public Task<bool> UpdateAsync(ElectionModel e, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(e.Id) || !_store.ContainsKey(e.Id)) return Task.FromResult(false);
        e.UpdatedUtc = DateTime.UtcNow;
        _store[e.Id] = e;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(string id, CancellationToken ct = default)
        => Task.FromResult(_store.TryRemove(id, out _));
}
