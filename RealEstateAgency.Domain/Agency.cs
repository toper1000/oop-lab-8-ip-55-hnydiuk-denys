using RealEstateAgency.Domain.Repositories;
using RealEstateAgency.Domain.Services;

namespace RealEstateAgency.Domain;

public class Agency
{
    public ClientRepository Clients { get; } = new();

    public PropertyRepository Properties { get; } = new();

    public ProposalRepository Proposals { get; } = new();

    private readonly SearchService _searchService = new();

    public SearchService Search => _searchService;

    public IReadOnlyList<string> SearchAll(string keyword)
        => _searchService.SearchAll(keyword, Clients, Properties);
}
