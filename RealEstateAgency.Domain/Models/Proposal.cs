namespace RealEstateAgency.Domain.Models;

public class Proposal
{
    private const int MaxProperties = 4;

    private readonly Client _client;

    private readonly List<RealEstateObject> _properties = new();
    private readonly HashSet<int> _rejectedIds = new();

    public Client Client => _client;

    public IReadOnlyList<RealEstateObject> Properties => _properties.AsReadOnly();

    public IReadOnlyCollection<int> RejectedIds => _rejectedIds;

    public Proposal(Client client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public void AddProperty(RealEstateObject property)
    {
        if (_properties.Count >= MaxProperties)
            throw new InvalidOperationException(
                $"A proposal may contain at most {MaxProperties} properties.");

        if (_properties.Any(p => p.Id == property.Id))
            throw new InvalidOperationException(
                "This property is already included in the proposal.");

        if (_rejectedIds.Contains(property.Id))
            throw new InvalidOperationException(
                "The client has already rejected this property and it cannot be re-added.");

        _properties.Add(property);
    }

    public void RejectProperty(int propertyId)
    {
        var property = _properties.FirstOrDefault(p => p.Id == propertyId)
            ?? throw new KeyNotFoundException(
                $"Property with ID {propertyId} was not found in this proposal.");

        _properties.Remove(property);
        _rejectedIds.Add(propertyId);
    }

    public string GetDisplayInfo()
    {
        if (_properties.Count == 0)
            return $"Proposal for {_client.Name} {_client.Surname}: (no properties)";

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Proposal for {_client.Name} {_client.Surname} ({_properties.Count}/{MaxProperties}):");
        foreach (var p in _properties)
            sb.AppendLine($"  • {p.GetDisplayInfo()}");

        return sb.ToString().TrimEnd();
    }
}
