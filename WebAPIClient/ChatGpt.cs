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
            Console.WriteLine("TrakaUser: " + trakaUser);

            var atsAutorisatie = atsVerbinding.ZoekSleutelAutorisatieVoorUser(trakaUser.Achternaam);
            Console.WriteLine("AtsAutorisatie gevonden: " + atsAutorisatie);

            if (atsAutorisatie != null)
            {
                await traka.Update(atsAutorisatie); //Bestaande gebruikers uit bronlijst bijwerken in sleutelkast
                Console.WriteLine("AtsAutorisatie bijgewerkt in Traka: " + atsAutorisatie);
            }
            else
            {
                await traka.DeleteUser(trakaUser); //Ontbrekende gebruikers uit bronlijst verwijderen uit sleutelkast
                Console.WriteLine("AtsAutorisatie verwijderd uit Traka: " + atsAutorisatie);
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