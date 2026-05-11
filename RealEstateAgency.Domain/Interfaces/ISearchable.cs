namespace RealEstateAgency.Domain.Interfaces;

public interface ISearchable
{
    bool ContainsKeyword(string keyword);

    string GetDisplayInfo();
}
