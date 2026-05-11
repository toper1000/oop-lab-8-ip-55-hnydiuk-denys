using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Interfaces;

namespace RealEstateAgency.Domain.Models;

public class RealEstateObject : ISearchable
{
    private static int _idCounter = 1;

    public int Id { get; }

    public PropertyType Type { get; private set; }

    public decimal Price { get; private set; }

    public string Address { get; private set; }

    public bool IsAvailable { get; private set; }

    public RealEstateObject(PropertyType type, decimal price, string address)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be positive.", nameof(price));
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.", nameof(address));

        Id = _idCounter++;
        Type = type;
        Price = price;
        Address = address;
        IsAvailable = true;
    }

    public void UpdateData(PropertyType type, decimal price, string address)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be positive.");
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.");

        Type = type;
        Price = price;
        Address = address;
    }

    public void SetAvailability(bool isAvailable) => IsAvailable = isAvailable;

    public bool ContainsKeyword(string keyword)
    {
        return Address.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || GetTypeLabel(Type).Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || Price.ToString().Contains(keyword);
    }

    public string GetDisplayInfo()
        => $"[ID:{Id}] {GetTypeLabel(Type)} | {Address} | Price: {Price:C} | Available: {IsAvailable}";

    public static string GetTypeLabel(PropertyType type) => type switch
    {
        PropertyType.Apartment1Room => "1-room apartment",
        PropertyType.Apartment2Room => "2-room apartment",
        PropertyType.Apartment3Room => "3-room apartment",
        PropertyType.PrivatePlot    => "Private plot",
        _                           => type.ToString()
    };
}
