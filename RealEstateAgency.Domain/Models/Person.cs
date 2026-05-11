namespace RealEstateAgency.Domain.Models;

public abstract class Person
{
    public string Name { get; protected set; }

    public string Surname { get; protected set; }

    protected Person(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException("Surname cannot be empty.", nameof(surname));

        Name = name;
        Surname = surname;
    }
}
