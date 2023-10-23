internal class ChatGpt
{
    private TrakaConnection traka;
    private AtsDatabaseVerbinding atsVerbinding;

    public ChatGpt()
    {
        traka = new TrakaConnection();
        atsVerbinding = new AtsDatabaseVerbinding();
    }

    internal async Task WerkAlleAutorisatiesInAts360BijInDeTrakaSleutelkast()
    {
        var pageSize = 10;
        var trakaUsers = await traka.GetListAsync(1, pageSize); //Ophalen van gebruikers in de sleutelkast

        foreach (var trakaUser in trakaUsers)
        {

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
}