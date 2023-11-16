using WebAPIClient;

static class Program
{
    public static TrakaRequestHandler RequestHandlerForTraka { get; } = new TrakaRequestHandler();

    static async Task Main()
    {
        Console.WriteLine("Press any key as Swagger is started");
        Console.ReadKey();

        await UpdateAutorisaties();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    static async Task UpdateAutorisaties()
    {
        try
        {
            var ai = new ChatGpt();
            //await ai.ControleerVersie();


            //await ai.LaatAlleAutorisatiesInTrakaZien();
            //await ai.LaatAlleSleutelsInTrakaZien();

            await ai.StuurAutorisatiesVanAtsNaarTrakaSleutelkast();
            await ai.OpschonenBestaandeAutorisatiesInTrakaSleutelkast();
            await ai.OphalenActueleSleutelStatus();

            await ai.LaatAlleAutorisatiesInTrakaZien();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
