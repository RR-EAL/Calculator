internal class AtsDatabaseVerbinding
{
    public AtsDatabaseVerbinding()
    {
    }

    public List<AtsSleutelAutorisatie> SleutelAutorisaties { get; internal set; } = new()
    {
        
        //new AtsSleutelAutorisatie { ForeignKey = "A88", Achternaam = "Rutgers", Voornaam = "Ravi", KastNummer="Kantine", SleutelPositie="A14", ExpirationDate = DateTime.Now.AddDays(7) },
        //new AtsSleutelAutorisatie { ForeignKey = "V89", Achternaam = "Rutgers", Voornaam = "Jeroen", KastNummer="Kantine", SleutelPositie="A15" },
        new AtsSleutelAutorisatie
        {
            ForeignKey = "X79",
            Achternaam = "Rutgers", Voornaam = "Ravi",
            KastNummer="Kantine", //mag mischien weg?
            SleutelPositie= null,
            Pasnummer = 79,
            StartTime = DateTime.Now.Date,
            ExpirationTime = DateTime.Now.Date.AddDays(7)
        },
        new AtsSleutelAutorisatie
        {
            ForeignKey = "X99",
            Achternaam = "Rutgers", Voornaam = "Ravi",
            KastNummer="Kantine", //mag mischien weg?
            SleutelPosities = new List<string> { "BEB8C045-B36B-40D5-ACE5-6626376BF0D4", "QEB8C045-B36B-40D5-ACE5-6626376BF0D4" },
            Pasnummer = 267,
            StartTime = DateTime.Now.Date,
            ExpirationTime = DateTime.Now.Date.AddDays(7)
        },
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
                Console.WriteLine($"Authorization {authorization.ForeignKey} is no longer valid.");
            }
            //Console.WriteLine(authorization.ToString());

            if (authorization.Achternaam == zoekWaarde)
                return authorization;
        }
        return null;
    }

    private bool CheckAuthorizationValidity(AtsSleutelAutorisatie authorization)
    {

        return DateTime.Now < authorization.ExpirationTime;
    }
}

public record AtsSleutelAutorisatie
{
    public string Achternaam { get; set; }
    public string Voornaam { get; set; }
    public string KastNummer { get; set; }
    public string SleutelPositie { get; set; }
    //public List<string> SleutelPosities { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime ExpirationTime { get; set; }
    public string ForeignKey { get; internal set; }
    public uint Pasnummer { get; internal set; }
}