using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

//Cabinet is een kast en een Fob is een sleutel positie
public class TrakaConnection
{
    private HttpClient httpClient;

    internal TrakaConnection()
    {
        // Initialize the HttpClient and API URL
        httpClient = new HttpClient();
    }


    internal async Task Update(AtsSleutelAutorisatie record)
    {
        try
        {
            bool isNotNew = await CheckIfAUserExists(record.Achternaam);
            if (isNotNew)
            {
            }
            else
            {
                await PostTrakaUser(record);
                //Werk voornaam bij : map pagina??..
            }
            await AssignNewSetOfPermissionsForSpecifiecUser(record);
        }
        catch (Exception ex)
        {

        }
    }

    private async Task<bool> CheckIfAUserExists(string userKey)
    {
        using HttpRequestMessage request = new(
        HttpMethod.Head,
        $"https://localhost:7252/Traka/User/foreignKey/{userKey}");



        using HttpResponseMessage response = await httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    //Map pagina???...
    private async Task AssignNewSetOfPermissionsForSpecifiecUser(AtsSleutelAutorisatie record)
    {
        var userKey = record.Achternaam;
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;

        HttpClient client = new HttpClient(handler);
        client.DefaultRequestHeaders.Accept.Clear();

        // Prepare the request data if you have one (e.g., for a POST request)
        var requestData = new
        {
            ItemIds = new List<string> { record.SleutelPositie },
            
        };

        var requestUrl = $"https://localhost:7252/Traka/User/foreignKey/{userKey}/ItemAccess";
        var antwoord = client.PostAsJsonAsync(requestUrl, requestData);
        
        var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        // Create the HTTP request message
        //try
        //{
        //    // Create a collection of key-value pairs for the form data
        //    var formData = new List<KeyValuePair<string, string>>
        //{
        //    new KeyValuePair<string, string>("key1", record.Achternaam), // Replace with your actual field names and values
        //    new KeyValuePair<string, string>("key2", record.Voornaam),
        //    new KeyValuePair<string, string>("key2", record.KastNummer),
        //    new KeyValuePair<string, string>("key2", record.SleutelPositie),
        //    new KeyValuePair<string, string>("key 3", record.ExpirationDate.ToString()),
        //};

        //    // Create the form content
        //    var formContent = new FormUrlEncodedContent(formData);

        //    // Send an HTTP POST request to the API
        //    HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7252/Traka/User", formContent);

        //    // Check if the request was successful
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string responseContent = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine("Post request successful. Response: " + responseContent);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Post request failed. Status Code: " + response.StatusCode);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("Error: " + ex.Message);
        //}
    }

    public async Task<List<MyTrakaUser>> GetListAsync(int page, int pageSize)
    {
        HttpClient httpClient = new HttpClient();
        using HttpResponseMessage response = await httpClient.GetAsync($"https://localhost:7252/Traka/User/page/{page}/pageSize/{pageSize}");
        return await response.Content.ReadFromJsonAsync<List<MyTrakaUser>>();
    }

    internal void DeleteExpiredAutorisation(string ongeldig)
    {
        throw new NotImplementedException();
    }

    //Map pagina
    private async Task PostTrakaUser(AtsSleutelAutorisatie record)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;

        HttpClient client = new HttpClient(handler);
        client.DefaultRequestHeaders.Accept.Clear();

        // Prepare the request data if you have one (e.g., for a POST request)
        var requestData = new
        {
            record.Voornaam,
            record.ForeignKey,
            Surname = record.Achternaam,
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
        HttpResponseMessage response = await client.SendAsync(response);

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

    internal async Task DeleteUser(MyTrakaUser trakaUser)
    {
        try
        {
            // Serialize the user object to JSON using System.Text.Json
            var userJson = JsonSerializer.Serialize(trakaUser);

            // Create a StringContent with JSON content
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");


            var authParameter =  Convert.ToBase64String(Encoding.UTF8.GetBytes("USER:PASSWORD"));
            // Send an HTTP DELETE request to the API
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7252/Traka/User/{trakaUser.surname}/foreignKey");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authParameter);
            requestMessage.Content = content;

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("User deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete the user. Server returned " + response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }



    public record MyTrakaUser
    {
        public string surname { get; set; }
        public string Forename { get; set; }
    }

}