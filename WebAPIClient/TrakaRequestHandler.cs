using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIClient
{
    public class TrakaRequestHandler : HttpClientHandler
    {
        public TrakaRequestHandler()
        {
            this.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes("traka:traka"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            return await base.SendAsync(request, cancellationToken);
        }

        static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
        {
            //var result = sslErrors == SslPolicyErrors.None;
            return true;
        }

    }
}
