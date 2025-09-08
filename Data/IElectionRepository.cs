namespace Election.API.Data;

using System.Threading;

using ElectionModel = Election.API.Models.Election;

public interface IElectionRepository
{
    Task<string> CreateAsync(ElectionModel election, CancellationToken ct = default);
    Task<ElectionModel?> GetAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyList<ElectionModel>> ListAsync(int limit = 50, string? status = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(ElectionModel election, CancellationToken ct = default);
    Task<bool> DeleteAsync(string id, CancellationToken ct = default);
}
