using System.Net;

internal class AtsDatabaseVerbinding
{
    public AtsDatabaseVerbinding()
    {
    }

    public List<AtsSleutelAutorisatie> SleutelAutorisaties { get; internal set; } = new()
    {
        new AtsSleutelAutorisatie { ForeigKey = "B55", Achternaam = "van den Hoek", Voornaam = "Rene", KastNummer="Kantine", SleutelPositie="A10", ExpirationDate = DateTime.Now.AddDays(7) },
        new AtsSleutelAutorisatie { ForeigKey = "A88", Achternaam = "Rutgers", Voornaam = "Ravi", KastNummer="Kantine", SleutelPositie="A14", ExpirationDate = DateTime.Now.AddDays(7) },
        new AtsSleutelAutorisatie { ForeigKey = "V89", Achternaam = "Rutgers", Voornaam = "Jeroen", KastNummer="Kantine", SleutelPositie="A15" },
        new AtsSleutelAutorisatie { ForeigKey = "X77", Achternaam = "sjaak", Voornaam = "henk", KastNummer="Kantine", SleutelPositie="A12" },
    };

    internal IEnumerable<AtsSleutelAutorisatie> ZoekPashouders(int page, int pageSize)
    {
        return SleutelAutorisaties.ToList();//.Skip(page).Take(pageSize).ToList();
    }

    internal AtsSleutelAutorisatie? ZoekSleutelAutorisatieVoorUser(string zoekWaarde)
    {
        foreach (var authorization in SleutelAutorisaties)
        {
            bool isValid = CheckAuthorizationValidity(authorization);

            if (!isValid)
            {
                // Handle the case where the authorization is no longer valid
                // For example, you can update its status, log the event, or take other actions
                Console.WriteLine($"Authorization {authorization.Id} is no longer valid.");
            }
            //Console.WriteLine(authorization.ToString());

            if (authorization.Achternaam == zoekWaarde)
                return authorization;
        }
        return null;
    }

    private bool CheckAuthorizationValidity(AtsSleutelAutorisatie authorization)
    {

        return DateTime.Now < authorization.ExpirationDate;
    }
}

public record AtsSleutelAutorisatie
{
    public string Achternaam { get; set; }
    public string Voornaam { get; set; }
    public string KastNummer { get; set; }
    public string SleutelPositie { get; set; }
    public DateTime ExpirationDate { get; internal set; }
    public string ForeigKey { get; internal set; }
}