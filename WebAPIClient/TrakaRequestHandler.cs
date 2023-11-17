using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebAPIClient
{
    public class TrakaRequestHandler : HttpClientHandler
    {
        public TrakaRequestHandler()
        {
            this.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;
        }

        public bool LogVerbose { get; set; } = false;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.ResetColor();
            ConsoleWriteLine("Request:");
            ConsoleWriteLine(request.ToString());
            if (request.Content != null)
            {
                ConsoleWriteLine(await request.Content.ReadAsStringAsync());
            }
            ConsoleWriteLine();

            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes("traka:traka"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }

            ConsoleWriteLine("Response:");
            ConsoleWriteLine(response.ToString());
            if (response.Content != null)
            {
                ConsoleWriteLine(await response.Content.ReadAsStringAsync());
            }
            ConsoleWriteLine();
            Console.ResetColor();

            return response;
        }

        static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
        {
            //var result = sslErrors == SslPolicyErrors.None;
            return true;
        }

        private void ConsoleWriteLine(string text = null)
        {
            if (!LogVerbose)
                return;

            if (text == null)
            {
                Console.WriteLine();
                return;
            }
            Console.WriteLine(text);
        }

    }
}
