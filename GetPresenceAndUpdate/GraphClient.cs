using GetPresenceAndUpdate;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class GraphClient
    {
        private string clientId = "YOU PUT YOUR CLIENT ID IN HERE";
          
        private GraphServiceClient theGraphClient;




    public GraphClient()
        {
            IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder
             .Create(clientId)             
             .WithRedirectUri("https://CLIENT ID OR SOMETHING ELSE YOU SET AS REDIRECT URI IN YOUR APP")
             .Build();

            InteractiveAuthenticationProvider authProvider = new InteractiveAuthenticationProvider(publicClientApplication);            
            theGraphClient = new GraphServiceClient(authProvider);
        }

        public async Task<PresenceInfo> GetPresence()
        {
            string requestUrl = "https://graph.microsoft.com/beta/me/presence";

            // Create the request message and add the content.
            HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            
            // Authenticate (add access token) our HttpRequestMessage
            await theGraphClient.AuthenticationProvider.AuthenticateRequestAsync(hrm);

            // Send the request and get the response.
            HttpResponseMessage response = await theGraphClient.HttpProvider.SendAsync(hrm);
            var content = await response.Content.ReadAsStringAsync();
            PresenceInfo presence = theGraphClient.HttpProvider.Serializer.DeserializeObject<PresenceInfo>(content);
            return presence;
        }
    }
}
