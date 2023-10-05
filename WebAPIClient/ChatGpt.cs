internal class ChatGpt
{
    private List<AtsSleutelAutorisatie> authorizations;

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

    internal void HouBijDatDeAutorisatieInTrakaNogGeldigIs()
    {
        try
        {
            // Iterate through the list of authorizations and check their validity
            foreach (var authorization in authorizations)
            {
                bool isValid = CheckAuthorizationValidity(authorization);

                if (!isValid)
                {
                    // Handle the case where the authorization is no longer valid
                    // For example, you can update its status, log the event, or take other actions
                    Console.WriteLine($"Authorization {authorization.Id} is no longer valid.");
                }
            }

            // You can add additional logic here as needed
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    private bool CheckAuthorizationValidity(AtsSleutelAutorisatie authorization)
    {
        return DateTime.Now < authorization.ExpirationDate;
    }
}