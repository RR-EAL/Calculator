using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;


//Cabinet is een kast en een Fob is een sleutel positie
internal class TrakaConnection
{


    private HttpClient httpClient;
    private string apiUrl; // Replace with your API URL

    internal TrakaConnection()
    {
        // Initialize the HttpClient and API URL
        httpClient = new HttpClient();
        apiUrl = "https://localhost:7252/Traka/FindAll"; // Replace with your actual API URL
    }


    internal void Update(AtsSleutelAutorisatie record)
    {
        try
        {
            PostTrakaUser().GetAwaiter().GetResult();
            //PostTrakaAutorisatie(record);
        }
        catch (Exception ex)
        {

        }
    }

    private async Task PostTrakaAutorisatie(AtsSleutelAutorisatie record)
    {
        try
        {
            // Create a collection of key-value pairs for the form data
            var formData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("key1", record.Achternaam), // Replace with your actual field names and values
            new KeyValuePair<string, string>("key2", record.Voornaam),
            new KeyValuePair<string, string>("key2", record.KastNummer),
            new KeyValuePair<string, string>("key2", record.SleutelPositie)
        };

            // Create the form content
            var formContent = new FormUrlEncodedContent(formData);

            // Send an HTTP POST request to the API
            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, formContent);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Post request successful. Response: " + responseContent);
            }
            else
            {
                Console.WriteLine("Post request failed. Status Code: " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }


    internal List<string> FindAllAuthorisations()
    {
        return null;
        //return authorisations;
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