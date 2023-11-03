internal class ChatGpt
{
    private TrakaConnection traka;
    private AtsDatabaseVerbinding atsVerbinding;

    public ChatGpt()
    {
        traka = new TrakaConnection();
        atsVerbinding = new AtsDatabaseVerbinding();
    }

    internal async Task StuurAutorisatiesVanAtsNaarTrakaSleutelkast()
    {
        var pageSize = 10;
        var page = 1;
        var pashouders = atsVerbinding.ZoekPashouders(page, pageSize);
        foreach (var p in pashouders)
        {
            await traka.Update(p);
        }
    }

    internal async Task OpschonenBestaandeAutorisatiesInTrakaSleutelkast()
    {
        var pageSize = 1000;
        var trakaUsers = await traka.GetListAsync(1, pageSize); //Ophalen van gebruikers in de sleutelkast

        foreach (var trakaUser in trakaUsers)
        {
            if (trakaUser.IsAdmin())
                continue;

            var atsAutorisatie = atsVerbinding.ZoekSleutelAutorisatieVoorUser(trakaUser.surname);
            Console.WriteLine("AtsAutorisatie gevonden: " + trakaUser);

            if (atsAutorisatie != null)
            {
                await traka.Update(atsAutorisatie); //Bestaande gebruikers uit bronlijst bijwerken in sleutelkast
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("AtsAutorisatie bijgewerkt in Traka");
                Console.ResetColor();
            }
            else
            {
                await traka.DeleteUser(trakaUser); //Ontbrekende gebruikers uit bronlijst verwijderen uit sleutelkast
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("AtsAutorisatie verwijderd uit Traka: " + atsAutorisatie);
                Console.ResetColor();
            }

            //HouBijDatDeAutorisatieInTrakaNogGeldigIs();
        }

        var ongeldigeAurisaties = new List<string>();

        if (ongeldigeAurisaties != null)
        {
            foreach (var ongeldig in ongeldigeAurisaties)
            {
                traka.DeleteExpiredAutorisation(ongeldig);
            }
        }
    }

    internal async Task OphalenActueleSleutelStatus()
    {
        //dit komt later
    }

    internal async Task ControleerVersie()
    {
        if (traka.GeefVersie() != "2.7.12.0")
            throw new NotSupportedException("Traka versie onjuist");
        //Todo
    }

    internal async Task LaatAlleAutorisatiesInTrakaZien()
    {
        foreach(var x in await traka.GetListAsync(1, 1000))
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(x);
            Console.ResetColor();
        }
    }
}