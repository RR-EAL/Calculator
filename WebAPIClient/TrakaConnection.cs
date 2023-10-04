using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;


//Cabinet is een kast en een Fob is een sleutel positie
internal class TrakaConnection
{
    internal void Update(AtsSleutelAutorisatie record)
    {
        try
        {
            PostTrakaUser().GetAwaiter().GetResult();
            PostTrakaAutorisatie();
        }
        catch (Exception ex)
        {

        }
    }

    private void PostTrakaAutorisatie()
    {
        throw new NotImplementedException();
    }

    internal List<string> FindAllAuthorisations()
    {
        throw new NotImplementedException();
    }

    internal void DeleteExpiredAutorisation(string ongeldig)
    {
        throw new NotImplementedException();
    }

    private async Task PostTrakaUser()
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;

        HttpClient client = new HttpClient(handler);
        client.DefaultRequestHeaders.Accept.Clear();

        // Prepare the request data if you have one (e.g., for a POST request)
        var requestData = new
        {
            ForeignKey = "value1",
            Forename = "value2",
            Surname = "sdaf",
            CardId = "1234",
            Pin = "1234",
            PinExpire = DateTime.Now,
            ActiveFlag = true,
            ActiveDate = DateTime.Now,
            ExpiryDate = DateTime.Now,
            Detail = "sdaf",
        };
        var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        // Create the HTTP request message
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7252/Traka/User");
        request.Content = requestContent; // Set the request content

        request.Content.ReadAsStringAsync();

        // Log the request details
        Console.WriteLine("Request:");
        Console.WriteLine($"Method: {request.Method}");
        Console.WriteLine($"URL: {request.RequestUri}");
        Console.WriteLine($"Headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
        Console.WriteLine($"Content: {await request.Content.ReadAsStringAsync()}");

        // Send the HTTP request
        HttpResponseMessage response = await client.SendAsync(request);

        // Log the response details
        Console.WriteLine("Response:");
        Console.WriteLine($"Status Code: {(int)response.StatusCode} {response.ReasonPhrase}");
        Console.WriteLine($"Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");

        if (response.IsSuccessStatusCode)
        {
            // Read and log the response content
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {responseContent}");
        }
        else
        {
            // Log an error message for non-successful responses
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }

    static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
    {
        var result = sslErrors == SslPolicyErrors.None;
        return true;
    }


}