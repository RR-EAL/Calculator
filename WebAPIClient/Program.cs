﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

HttpClientHandler handler = new HttpClientHandler();

    // Set custom server validation callback
handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;

    // Create an HttpClient object
HttpClient client = new HttpClient(handler);
client.DefaultRequestHeaders.Accept.Clear();

//Ophalen welke kasten zijn er
//Ophalen welke sleutels zitten er in welke kast
//Maak user aan
//Geef user toegang tot sleutel in kast

    var json = await client.GetStringAsync(
         "https://localhost:7252/WeatherForecast");

Console.WriteLine("Press any key");
Console.ReadKey();


 static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
{
    //var result = sslErrors == SslPolicyErrors.None;
    return true;
}