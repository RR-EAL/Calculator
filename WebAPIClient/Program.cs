﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

static class Program
{
    static async Task Main()
    {
        Console.WriteLine("Press any key as Swagger is started");
        Console.ReadKey();

        UpdateAutorisaties();

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    static void UpdateAutorisaties()
    {
        var ai = new ChatGpt();
        ai.WerkAlleAutorisatiesInAts360BijInDeTrakaSleutelkast();
    }


}
