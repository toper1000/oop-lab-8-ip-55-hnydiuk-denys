using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Enums;
using RealEstateAgency.Domain.Models;

var agency = new Agency();
bool running = true;

while (running)
{
    Console.Clear();
    Console.WriteLine("========================================");
    Console.WriteLine("        REAL ESTATE AGENCY              ");
    Console.WriteLine(" -------------------------------------- ");
    Console.WriteLine("   1 - Client Management                ");
    Console.WriteLine("   2 - Property Management              ");
    Console.WriteLine("   3 - Proposal Management              ");
    Console.WriteLine("   4 - Search                           ");
    Console.WriteLine("   0 - Exit                             ");
    Console.WriteLine("========================================");
    Console.Write("Your choice: ");

    switch (ReadLine())
    {
        case "1": ClientMenu(); break;
        case "2": PropertyMenu(); break;
        case "3": ProposalMenu(); break;
        case "4": SearchMenu(); break;
        case "0": running = false; break;
        default:  Pause("Unknown command."); break; //
    }
}

void ClientMenu()
{
    bool back = false;
    while (!back)
    {
        Console.Clear();
        PrintHeader("CLIENT MANAGEMENT");
        Console.WriteLine("  1 - Add client");
        Console.WriteLine("  2 - Remove client");
        Console.WriteLine("  3 - Update client");
        Console.WriteLine("  4 - View client");
        Console.WriteLine("  5 - List all clients");
        Console.WriteLine("  6 - Set search criteria for client");
        Console.WriteLine("  0 - Back");
        Console.Write("Your choice: ");

        switch (ReadLine())
        {
            case "1": AddClient(); break;
            case "2": RemoveClient(); break;
            case "3": UpdateClient(); break;
            case "4": ViewClient(); break;
            case "5": ListClients(); break;
            case "6": SetClientCriteria(); break;
            case "0": back = true; break;
            default:  Pause("Unknown command."); break;
        }
    }
}

void AddClient()
{
    Console.Clear();
    PrintHeader("ADD CLIENT");
    try
    {
        string name    = Ask("First name");
        string surname = Ask("Surname");
        string account = Ask("Bank account number");
        agency.Clients.Add(new Client(name, surname, account));
        Pause("Client added.");
    }
    catch (ArgumentException ex) { Pause($"Error: {ex.Message}"); }
}

void RemoveClient()
{
    Console.Clear();
    PrintHeader("REMOVE CLIENT");
    string account = Ask("Client bank account");
    if (agency.Clients.Remove(account))
    {
        agency.Proposals.Remove(account);
        Pause("Client removed.");
    }
    else Pause("Client not found.");
}

void UpdateClient()
{
    Console.Clear();
    PrintHeader("UPDATE CLIENT");
    string account = Ask("Client bank account to search");
    var client = agency.Clients.FindByBankAccount(account);
    if (client is null) { Pause("Client not found."); return; }

    Console.WriteLine($"  Current data: {client.GetDisplayInfo()}");
    try
    {
        string name    = Ask("New first name");
        string surname = Ask("New surname");
        string newAcc  = Ask("New account number");
        client.UpdateData(name, surname, newAcc);
        Pause("Data updated.");
    }
    catch (ArgumentException ex) { Pause($"Error: {ex.Message}"); }
}

void ViewClient()
{
    Console.Clear();
    PrintHeader("VIEW CLIENT");
    string account = Ask("Account number");
    var client = agency.Clients.FindByBankAccount(account);
    if (client is null) { Pause("Client not found."); return; }
    Console.WriteLine($"  {client.GetDisplayInfo()}");
    Pause();
}

void ListClients()
{
    bool back = false;
    while (!back)
    {
        Console.Clear();
        PrintHeader("CLIENT LIST");
        Console.WriteLine("  1 - Default");
        Console.WriteLine("  2 - Sort by name");
        Console.WriteLine("  3 - Sort by surname");
        Console.WriteLine("  4 - Sort by first digit of account");
        Console.WriteLine("  0 - Back");
        Console.Write("Your choice: ");

        var choice = ReadLine();
        if (choice == "0") { back = true; continue; }

        var list = choice switch
        {
            "1" => agency.Clients.GetAll(),
            "2" => agency.Clients.GetSortedByName(),
            "3" => agency.Clients.GetSortedBySurname(),
            "4" => agency.Clients.GetSortedByBankAccountFirstDigit(),
            _   => null
        };

        if (list is null) { Pause("Unknown command."); continue; }

        Console.Clear();
        if (list.Count == 0) { Pause("List is empty."); continue; }
        for (int i = 0; i < list.Count; i++)
            Console.WriteLine($"  {i + 1}. {list[i].GetDisplayInfo()}");
        Pause();
    }
}

void SetClientCriteria()
{
    Console.Clear();
    PrintHeader("CLIENT SEARCH CRITERIA");
    string account = Ask("Client bank account");
    var client = agency.Clients.FindByBankAccount(account);
    if (client is null) { Pause("Client not found."); return; }

    PropertyType? type = AskPropertyType();
    if (type is null) return;

    try
    {
        string priceStr = Ask("Maximum price");
        if (!decimal.TryParse(priceStr, out decimal price))
            { Pause("Invalid price format."); return; }
        client.SetDesiredCriteria(type.Value, price);
        Pause("Criteria set.");
    }
    catch (ArgumentException ex) { Pause($"{ex.Message}"); }
}

void PropertyMenu()
{
    bool back = false;
    while (!back)
    {
        Console.Clear();
        PrintHeader("PROPERTY MANAGEMENT");
        Console.WriteLine("  1 - Add property");
        Console.WriteLine("  2 - Remove property");
        Console.WriteLine("  3 - Update property");
        Console.WriteLine("  4 - View property");
        Console.WriteLine("  5 - List all properties");
        Console.WriteLine("  0 - Back");
        Console.Write("Your choice: ");

        switch (ReadLine())
        {
            case "1": AddProperty(); break;
            case "2": RemoveProperty(); break;
            case "3": UpdateProperty(); break;
            case "4": ViewProperty(); break;
            case "5": ListProperties(); break;
            case "0": back = true; break;
            default:  Pause("Unknown command."); break;
        }
    }
}

void AddProperty()
{
    Console.Clear();
    PrintHeader("ADD PROPERTY");

    PropertyType? type = AskPropertyType();
    if (type is null) return;

    try
    {
        string priceStr = Ask("Price");
        if (!decimal.TryParse(priceStr, out decimal price))
            { Pause("Invalid price format."); return; }
        string address = Ask("Address");
        agency.Properties.Add(new RealEstateObject(type.Value, price, address));
        Pause("Property added.");
    }
    catch (ArgumentException ex) { Pause($"{ex.Message}"); }
}

void RemoveProperty()
{
    Console.Clear();
    PrintHeader("REMOVE PROPERTY");
    string idStr = Ask("Property ID");
    if (!int.TryParse(idStr, out int id)) { Pause("Invalid ID."); return; }
    Pause(agency.Properties.Remove(id) ? "Property removed." : "Property not found.");
}

void UpdateProperty()
{
    Console.Clear();
    PrintHeader("UPDATE PROPERTY");
    string idStr = Ask("Property ID");
    if (!int.TryParse(idStr, out int id)) { Pause("Invalid ID."); return; }
    var prop = agency.Properties.FindById(id);
    if (prop is null) { Pause("Property not found."); return; }

    Console.WriteLine($"  Current data: {prop.GetDisplayInfo()}");
    PropertyType? type = AskPropertyType();
    if (type is null) return;

    try
    {
        string priceStr = Ask("New price");
        if (!decimal.TryParse(priceStr, out decimal price))
            { Pause("Invalid price format."); return; }
        string address = Ask("New address");
        prop.UpdateData(type.Value, price, address);
        Pause("Data updated.");
    }
    catch (ArgumentException ex) { Pause($"{ex.Message}"); }
}

void ViewProperty()
{
    Console.Clear();
    PrintHeader("VIEW PROPERTY");
    string idStr = Ask("Property ID");
    if (!int.TryParse(idStr, out int id)) { Pause("Invalid ID."); return; }
    var prop = agency.Properties.FindById(id);
    if (prop is null) { Pause("Property not found."); return; }
    Console.WriteLine($"  {prop.GetDisplayInfo()}");
    Pause();
}

void ListProperties()
{
    bool back = false;
    while (!back)
    {
        Console.Clear();
        PrintHeader("PROPERTY LIST");
        Console.WriteLine("  1 - Default");
        Console.WriteLine("  2 - Sort by type");
        Console.WriteLine("  3 - Sort by price");
        Console.WriteLine("  0 - Back");
        Console.Write("Your choice: ");

        var choice = ReadLine();
        if (choice == "0") { back = true; continue; }

        var list = choice switch
        {
            "1" => agency.Properties.GetAll(),
            "2" => agency.Properties.GetSortedByType(),
            "3" => agency.Properties.GetSortedByPrice(),
            _   => null
        };

        if (list is null) { Pause("Unknown command."); continue; }

        Console.Clear();
        if (list.Count == 0) { Pause("List is empty."); continue; }
        foreach (var p in list)
            Console.WriteLine($"  {p.GetDisplayInfo()}");
        Pause();
    }
}

void ProposalMenu()
{
    bool back = false;
    while (!back)
    {
        Console.Clear();
        PrintHeader("PROPOSAL MANAGEMENT");
        Console.WriteLine("  1 - Add property to client proposal");
        Console.WriteLine("  2 - Reject property from proposal");
        Console.WriteLine("  3 - View client proposal");
        Console.WriteLine("  4 - Check desired property availability");
        Console.WriteLine("  0 - Back");
        Console.Write("Your choice: ");

        switch (ReadLine())
        {
            case "1": ProposalAdd(); break;
            case "2": ProposalReject(); break;
            case "3": ProposalView(); break;
            case "4": ProposalCheck(); break;
            case "0": back = true; break;
            default:  Pause("Unknown command."); break;
        }
    }
}

void ProposalAdd()
{
    Console.Clear();
    PrintHeader("ADD PROPERTY TO PROPOSAL");
    var client = FindClientByAccount();
    if (client is null) return;

    string idStr = Ask("Property ID");
    if (!int.TryParse(idStr, out int id)) { Pause("Invalid ID."); return; }
    var prop = agency.Properties.FindById(id);
    if (prop is null) { Pause("Property not found."); return; }

    try
    {
        var proposal = agency.Proposals.GetOrCreate(client);
        proposal.AddProperty(prop);
        Pause("Property added to proposal.");
    }
    catch (InvalidOperationException ex) { Pause($"{ex.Message}"); }
}

void ProposalReject()
{
    Console.Clear();
    PrintHeader("REJECT PROPERTY");
    var client = FindClientByAccount();
    if (client is null) return;

    var proposal = agency.Proposals.Get(client.BankAccount);
    if (proposal is null) { Pause("No proposal found for this client."); return; }

    Console.WriteLine();
    Console.WriteLine(proposal.GetDisplayInfo());
    string idStr = Ask("Property ID to reject");
    if (!int.TryParse(idStr, out int id)) { Pause("Invalid ID."); return; }

    try
    {
        proposal.RejectProperty(id);
        Pause("Property rejected.");
    }
    catch (KeyNotFoundException ex) { Pause($"{ex.Message}"); }
}

void ProposalView()
{
    Console.Clear();
    PrintHeader("VIEW PROPOSAL");
    var client = FindClientByAccount();
    if (client is null) return;

    var proposal = agency.Proposals.Get(client.BankAccount);
    if (proposal is null) { Pause("Proposal not found."); return; }
    Console.WriteLine();
    Console.WriteLine(proposal.GetDisplayInfo());
    Pause();
}

void ProposalCheck()
{
    Console.Clear();
    PrintHeader("CHECK DESIRED PROPERTY AVAILABILITY");
    var client = FindClientByAccount();
    if (client is null) return;

    if (client.DesiredPropertyType is null || client.DesiredMaxPrice is null)
    {
        Pause("Client has no criteria set.\n   Set them via the Client menu → option 6.");
        return;
    }

    var match = agency.Properties.FindMatch(
        client.DesiredPropertyType.Value,
        client.DesiredMaxPrice.Value);

    Console.WriteLine();
    Console.WriteLine($"  Client criteria: {client.DesiredPropertyType}, max {client.DesiredMaxPrice:N0}");
    Console.WriteLine();

    if (match is not null)
        Console.WriteLine($"  Found: {match.GetDisplayInfo()}");
    else
        Console.WriteLine("  No matching property found.");

    Pause();
}

void SearchMenu()
{
    bool back = false;
    while (!back)
    {
        Console.Clear();
        PrintHeader("SEARCH");
        Console.WriteLine("  1 - Search clients");
        Console.WriteLine("  2 - Search properties");
        Console.WriteLine("  3 - Search all");
        Console.WriteLine("  4 - Advanced client search");
        Console.WriteLine("  0 - Back");
        Console.Write("Your choice: ");

        switch (ReadLine())
        {
            case "1": SearchClients(); break;
            case "2": SearchProperties(); break;
            case "3": SearchAll(); break;
            case "4": AdvancedSearch(); break;
            case "0": back = true; break;
            default:  Pause("Unknown command."); break;
        }
    }
}

void SearchClients()
{
    Console.Clear();
    PrintHeader("CLIENT SEARCH");
    string kw = Ask("Keyword");
    var results = agency.Search.SearchClients(kw, agency.Clients)
                               .Select(c => c.GetDisplayInfo()).ToList();
    PrintSearchResults(results);
}

void SearchProperties()
{
    Console.Clear();
    PrintHeader("PROPERTY SEARCH");
    string kw = Ask("Keyword");
    var results = agency.Search.SearchProperties(kw, agency.Properties)
                               .Select(p => p.GetDisplayInfo()).ToList();
    PrintSearchResults(results);
}

void SearchAll()
{
    Console.Clear();
    PrintHeader("SEARCH ALL");
    string kw = Ask("Keyword");
    PrintSearchResults(agency.SearchAll(kw).ToList());
}

void AdvancedSearch()
{
    Console.Clear();
    PrintHeader("ADVANCED CLIENT SEARCH");
    Console.WriteLine("  (Leave blank to skip a criterion)");
    Console.WriteLine();

    string? surname = Ask("Surname (or Enter)");
    if (string.IsNullOrWhiteSpace(surname)) surname = null;

    Console.WriteLine("  Desired property type:");
    Console.WriteLine("  1 - 1-room  2 - 2-room  3 - 3-room  4 - Plot  Enter - any");
    Console.Write("  Choice: ");

    PropertyType? type = Console.ReadLine()?.Trim() switch
    {
        "1" => PropertyType.Apartment1Room,
        "2" => PropertyType.Apartment2Room,
        "3" => PropertyType.Apartment3Room,
        "4" => PropertyType.PrivatePlot,
        _   => null
    };

    var results = agency.Search.AdvancedSearch(agency.Clients, surname, type)
                               .Select(c => c.GetDisplayInfo()).ToList();
    PrintSearchResults(results);
}

Client? FindClientByAccount()
{
    string account = Ask("Client bank account");
    var client = agency.Clients.FindByBankAccount(account);
    if (client is null) Pause("Client not found.");
    return client;
}

PropertyType? AskPropertyType()
{
    Console.WriteLine("  Property type:");
    Console.WriteLine("    1 - 1-room apartment");
    Console.WriteLine("    2 - 2-room apartment");
    Console.WriteLine("    3 - 3-room apartment");
    Console.WriteLine("    4 - Private plot");
    Console.Write("  Choice: ");

    PropertyType? type = Console.ReadLine()?.Trim() switch
    {
        "1" => PropertyType.Apartment1Room,
        "2" => PropertyType.Apartment2Room,
        "3" => PropertyType.Apartment3Room,
        "4" => PropertyType.PrivatePlot,
        _   => null
    };

    if (type is null) Pause("Invalid type.");
    return type;
}

void PrintSearchResults(List<string> results)
{
    Console.WriteLine();
    if (results.Count == 0)
        Console.WriteLine("  Nothing found.");
    else
        for (int i = 0; i < results.Count; i++)
            Console.WriteLine($"  {i + 1}. {results[i]}");
    Pause();
}

void PrintHeader(string title)
{
    Console.WriteLine($"-- {title} --");
    Console.WriteLine();
}

string Ask(string prompt)
{
    Console.Write($"{prompt}: ");
    return Console.ReadLine() ?? string.Empty;
}

string ReadLine() => Console.ReadLine()?.Trim() ?? string.Empty;

void Pause(string message = "")
{
    if (!string.IsNullOrEmpty(message)) Console.WriteLine($"\n{message}");
    Console.Write("\n  [Press Enter to continue]");
    Console.ReadLine();
}
