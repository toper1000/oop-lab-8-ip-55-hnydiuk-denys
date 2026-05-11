using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Models;
using RealEstateAgency.Domain.Repositories;

namespace RealEstateAgency.Domain.Services;

public class SearchService
{
    public IReadOnlyList<Client> SearchClients(string keyword, ClientRepository clientRepository)
        => clientRepository.Search(keyword);

    public IReadOnlyList<RealEstateObject> SearchProperties(string keyword, PropertyRepository propertyRepository)
        => propertyRepository.Search(keyword);

    public IReadOnlyList<string> SearchAll(
        string keyword,
        ClientRepository clientRepository,
        PropertyRepository propertyRepository)
    {
        var results = new List<string>();

        results.AddRange(
            clientRepository.Search(keyword)
                            .Select(c => $"[Client]   {c.GetDisplayInfo()}"));

        results.AddRange(
            propertyRepository.Search(keyword)
                              .Select(p => $"[Property] {p.GetDisplayInfo()}"));

        return results;
    }

    public IReadOnlyList<Client> AdvancedSearch(
        ClientRepository clientRepository,
        string? surname = null,
        PropertyType? desiredType = null)
    {
        return clientRepository.GetAll()
            .Where(c =>
                (surname is null
                    || c.Surname.Contains(surname, StringComparison.OrdinalIgnoreCase))
                && (desiredType is null
                    || c.DesiredPropertyType == desiredType))
            .ToList();
    }
}
