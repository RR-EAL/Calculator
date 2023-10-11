internal class ChatGpt
{
    private TrakaConnection traka;
    private List<AtsSleutelAutorisatie> authorizations;
    public ChatGpt()
    {
        traka = new TrakaConnection();
    }

    internal void WerkAlleAutorisatiesInAts360BijInDeTrakaSleutelkast()
    {
        var autorisatiesInTraka = traka.FindAllUsers();

        foreach (var autorisatie in autorisatiesInTraka)
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
        authorizations = traka.FindAllUsers();
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
                //Console.WriteLine(authorization.ToString());
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