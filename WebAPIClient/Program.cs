using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
        var ai = new ChatGpt();
        await ai.ControleerVersie();
       

        await ai.LaatAlleAutorisatiesInTrakaZien();
        //await ai.LaatAlleSleutelsInTrakaZien();

        await ai.StuurAutorisatiesVanAtsNaarTrakaSleutelkast();
        //await ai.OpschonenBestaandeAutorisatiesInTrakaSleutelkast();
        await ai.OphalenActueleSleutelStatus();

        await ai.LaatAlleAutorisatiesInTrakaZien();
    }
}
