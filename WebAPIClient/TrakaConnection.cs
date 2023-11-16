using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

//Cabinet is een kast en een Fob is een sleutel positie
public class TrakaConnection
{
    private HttpClient httpClient;
    //private string baseUrl = "https://eal-trakaweb:10700";
    private string baseUrl = "https://localhost:7252";
    

    internal TrakaConnection()
    {

        // Set custom server validation callback

        // Initialize the HttpClient and API URL
        httpClient = CreateClient();
    }


    internal async Task Update(AtsSleutelAutorisatie record)
    {
        bool isNotNew = await CheckIfAUserExists(record.ForeignKey);
        if (isNotNew)
        {
            await PutTrakaUser(record);
        }
        else
        {
            await PostTrakaUser(record);
            //Werk voornaam bij : map pagina??..
        }
        await AssignNewSetOfPermissionsForSpecifiecUser(record);
    }

    private async Task PutTrakaUser(AtsSleutelAutorisatie record)
    {
        var client = CreateClient();
        var requestData = new
        {

            Forename = record.Voornaam,
            record.ForeignKey,
            Surname = record.Achternaam,
            CardId = record.Pasnummer.ToString("000000"),
            ActiveDate = DateTime.Now,
            ExpiryDate = DateTime.Now.AddYears(1)
        };
        var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/Traka/User/ForeignKey/{record.ForeignKey}");
        request.Content = requestContent; // Set the request content

        request.Content.ReadAsStringAsync();

        HttpResponseMessage response = await client.SendAsync(request);

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new NotSupportedException("PutTrakaUser failed", ex);
        }
    }

    private async Task<bool> CheckIfAUserExists(string userKey)
    {
        using HttpRequestMessage request = new(
        HttpMethod.Head,
        $"{baseUrl}/Traka/User/foreignKey/{userKey}");



        using HttpResponseMessage response = await httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    //Map pagina???...
    private async Task AssignNewSetOfPermissionsForSpecifiecUser(AtsSleutelAutorisatie record)
    {
        var userKey = record.ForeignKey;

        HttpClient client = CreateClient();

        // Prepare the request data if you have one (e.g., for a POST request)
        var requestData = new
        {
            ItemIds = new List<string> { record.SleutelPositie },

        };

        //blz 101
        var requestUrl = $"{baseUrl}/Traka/User/foreignKey/{userKey}/ItemAccess";
        var antwoord = await client.PostAsJsonAsync(requestUrl, requestData);

        antwoord.EnsureSuccessStatusCode();

        //var requestContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
    }

    public async Task<List<MyTrakaUser>> GetListAsync(int page, int pageSize)
    {
        var httpClient = CreateClient();

        using HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/Traka/User/page/{page}/pageSize/{pageSize}");
        return await response.Content.ReadFromJsonAsync<List<MyTrakaUser>>();
    }

    internal void DeleteExpiredAutorisation(string ongeldig)
    {
        throw new NotImplementedException();
    }

    private HttpClient CreateClient()
    {
        return new HttpClient(Program.RequestHandlerForTraka);
    }

    //Map pagina
    private async Task PostTrakaUser(AtsSleutelAutorisatie record)
    {
        var client = CreateClient();
        //client.DefaultRequestHeaders.Accept.Clear();

        // Prepare the request data if you have one (e.g., for a POST request)
        var requestData = new
        {

            Forename = record.Voornaam,
            record.ForeignKey,
            Surname = record.Achternaam,
            CardId = record.Pasnummer.ToString("000000"),
            //Pin = ",
            PinExpire = DateTime.Now,
            ActiveFlag = true,
            ActiveDate = DateTime.Now,
            ExpiryDate = DateTime.Now.AddYears(1),
            Detail = "sdaf",
        };
        // Create the HTTP request message

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/Traka/User")
        {
            Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json")
        };

        // Send the HTTP request
        HttpResponseMessage response = await client.SendAsync(request);


        response.EnsureSuccessStatusCode();
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

            // Send an HTTP DELETE request to the API
            string requestUrl = $"{baseUrl}/Traka/User/foreignKey/{trakaUser.ForeignKey}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
            requestMessage.Content = content;
            return;

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

    internal async Task<string> GeefVersie()
    {
        var httpClient = CreateClient();

        using HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/Traka/Version");
        return await response.Content.ReadAsStringAsync();
    }


    internal async Task<string> GetKeyList()
    {
        var httpClient = CreateClient();

        using HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/Traka/Version");
        return await response.Content.ReadAsStringAsync();
    }


    internal async Task GetItemListAsync(string userKey)
    {
        var client = CreateClient();

        using HttpResponseMessage response = await client.GetAsync($"{baseUrl}/Traka/user/foreignKey/{userKey}/ItemAccess");
        var data = await response.Content.ReadAsStringAsync(); //.ReadFromJsonAsync<List<MyTrakaUser>>();
    }

    internal async Task<IEnumerable<object>> GetFobStatusAsync(int page, int pageSize)
    {
        var httpClient = CreateClient();

        using HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/Traka/ifob/page/{page}/pageSize/{pageSize}");
        return await response.Content.ReadFromJsonAsync<List<MyFobStatus>>();
    }

    public record MyTrakaUser
    {
        public string ForeignKey { get; set; }
        public string surname { get; set; }
        public string Forename { get; set; }

        internal bool IsAdmin()
        {
            return string.IsNullOrWhiteSpace(ForeignKey);
        }
    }

}