using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Domain.Repositories;

public class ClientRepository
{
    private readonly List<Client> _clients = new();

    public void Add(Client client)
    {
        if (client is null) throw new ArgumentNullException(nameof(client));
        _clients.Add(client);
    }

    public bool Remove(string bankAccount)
    {
        var client = FindByBankAccount(bankAccount);
        return client is not null && _clients.Remove(client);
    }

    public Client? FindByBankAccount(string bankAccount)
        => _clients.FirstOrDefault(c => c.BankAccount == bankAccount);

    public IReadOnlyList<Client> GetAll() => _clients.AsReadOnly();

    public IReadOnlyList<Client> GetSortedByName()
        => _clients.OrderBy(c => c.Name).ToList();

    public IReadOnlyList<Client> GetSortedBySurname()
        => _clients.OrderBy(c => c.Surname).ToList();

    public IReadOnlyList<Client> GetSortedByBankAccountFirstDigit()
        => _clients.OrderBy(c => c.BankAccount.FirstOrDefault()).ToList();

    public IReadOnlyList<Client> Search(string keyword)
        => _clients.Where(c => c.ContainsKeyword(keyword)).ToList();
}
