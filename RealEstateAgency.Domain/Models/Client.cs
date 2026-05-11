using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Interfaces;

namespace RealEstateAgency.Domain.Models;

public class Client : Person, ISearchable
{
    public string BankAccount { get; private set; }

    public PropertyType? DesiredPropertyType { get; private set; }

    public decimal? DesiredMaxPrice { get; private set; }

    public Client(string name, string surname, string bankAccount) : base(name, surname)
    {
        if (string.IsNullOrWhiteSpace(bankAccount))
            throw new ArgumentException("Bank account cannot be empty.", nameof(bankAccount));

        BankAccount = bankAccount;
    }

    public void UpdateData(string name, string surname, string bankAccount)
    {
        if (string.IsNullOrWhiteSpace(name))        throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(surname))     throw new ArgumentException("Surname cannot be empty.");
        if (string.IsNullOrWhiteSpace(bankAccount)) throw new ArgumentException("Bank account cannot be empty.");

        Name = name;
        Surname = surname;
        BankAccount = bankAccount;
    }

    public void SetDesiredCriteria(PropertyType type, decimal maxPrice)
    {
        if (maxPrice <= 0)
            throw new ArgumentException("Max price must be positive.", nameof(maxPrice));

        DesiredPropertyType = type;
        DesiredMaxPrice = maxPrice;
    }

    public bool ContainsKeyword(string keyword)
    {
        return Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || Surname.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || BankAccount.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }

    public string GetDisplayInfo()
    {
        var desired = DesiredPropertyType.HasValue
            ? $" | Wants: {DesiredPropertyType} ≤ {DesiredMaxPrice:C}"
            : string.Empty;

        return $"{Name} {Surname} | Account: {BankAccount}{desired}";
    }
}
