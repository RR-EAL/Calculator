internal class AtsDatabaseVerbinding
{
    public AtsDatabaseVerbinding()
    {
    }

    public List<AtsSleutelAutorisatie> SleutelAutorisaties { get; internal set; } = new()
    {
        new AtsSleutelAutorisatie { Achternaam = "van den Hoek", Voornaam = "Rene", KastNummer="Kantine", SleutelPositie="A10" },
        new AtsSleutelAutorisatie { Achternaam = "Rutgers", Voornaam = "Ravi", KastNummer="Kantine", SleutelPositie="A14" },
    };
}

internal record AtsSleutelAutorisatie
{
    public string Achternaam { get; set; }
    public string Voornaam { get; set; }
    public string KastNummer { get; set; }
    public string SleutelPositie { get; set; }
    public DateTime ExpirationDate { get; internal set; }
    public object Id { get; internal set; }
}