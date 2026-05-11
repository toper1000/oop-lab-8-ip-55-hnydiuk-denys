using RealEstateAgency.Domain.Models;

namespace RealEstateAgency.Domain.Repositories;

public class ProposalRepository
{
    private readonly Dictionary<string, Proposal> _proposals = new();

    public Proposal GetOrCreate(Client client)
    {
        if (!_proposals.TryGetValue(client.BankAccount, out var proposal))
        {
            proposal = new Proposal(client);
            _proposals[client.BankAccount] = proposal;
        }
        return proposal;
    }

    public Proposal? Get(string bankAccount)
        => _proposals.GetValueOrDefault(bankAccount);

    public bool Remove(string bankAccount)
        => _proposals.Remove(bankAccount);
}
