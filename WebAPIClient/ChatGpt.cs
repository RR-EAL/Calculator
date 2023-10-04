internal class ChatGpt
{
    private AtsDatabaseVerbinding atsData;
    private TrakaConnection traka;
    public ChatGpt()
    {
        atsData = new AtsDatabaseVerbinding();
        traka = new TrakaConnection();
    }

    internal void WerkAlleAutorisatiesInAts360BijInDeTrakaSleutelkast()
    {
        var autorisatiesInTraka = traka.FindAllAuthorisations();

        foreach (var autorisatie in atsData.SleutelAutorisaties)
        {
            traka.Update(autorisatie);
            HouBijDatDeAutorisatieInTrakaNogGeldigIs();
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

    private void HouBijDatDeAutorisatieInTrakaNogGeldigIs()
    {
        throw new NotImplementedException();
    }
}