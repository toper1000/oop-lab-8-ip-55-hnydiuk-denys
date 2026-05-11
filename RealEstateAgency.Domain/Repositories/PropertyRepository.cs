using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Domain.Repositories;

public class PropertyRepository
{
    private readonly List<RealEstateObject> _properties = new();

    public void Add(RealEstateObject property)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));
        _properties.Add(property);
    }

    public bool Remove(int id)
    {
        var property = FindById(id);
        return property is not null && _properties.Remove(property);
    }

    public RealEstateObject? FindById(int id)
        => _properties.FirstOrDefault(p => p.Id == id);

    public IReadOnlyList<RealEstateObject> GetAll() => _properties.AsReadOnly();

    public IReadOnlyList<RealEstateObject> GetSortedByType()
        => _properties.OrderBy(p => p.Type).ToList();

    public IReadOnlyList<RealEstateObject> GetSortedByPrice()
        => _properties.OrderBy(p => p.Price).ToList();

    public IReadOnlyList<RealEstateObject> Search(string keyword)
        => _properties.Where(p => p.ContainsKeyword(keyword)).ToList();

    public RealEstateObject? FindMatch(PropertyType type, decimal maxPrice)
        => _properties.FirstOrDefault(p => p.Type == type && p.Price <= maxPrice && p.IsAvailable);
}
