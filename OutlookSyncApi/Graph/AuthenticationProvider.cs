using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using OutlookSyncApi.Models;
using OutlookSyncApi.Repositories;

namespace OutlookSyncApi
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IPublicClientApplication msalClient;
        private readonly string[] scopes;
        private IEnumerable<IAccount> accounts;
        private GraphConfiguration graphConfiguration;

        public AuthenticationProvider(IUnitOfWork unitOfWork)
        {
            graphConfiguration = unitOfWork.GraphConfigurations.GetAll().FirstOrDefault();
            scopes = graphConfiguration.Scopes.Split(" ");
            msalClient = PublicClientApplicationBuilder
                .Create(graphConfiguration.ClientId)
                .WithAuthority(graphConfiguration.Authority)
                .Build();
        }

        public async Task<string> GetAccessToken()
        {
            accounts = await msalClient.GetAccountsAsync();
            // If there is no saved user account, the user must sign-in
            AuthenticationResult result = null;
            if (accounts.Any())
            {
                result = await msalClient.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                  .ExecuteAsync();
            }
            else
            {
                try
                {
                    var securePassword = new SecureString();
                    foreach (char c in graphConfiguration.Password)
                    {
                        securePassword.AppendChar(c);
                    }


                    result = msalClient.AcquireTokenByUsernamePassword(scopes, graphConfiguration.Username, securePassword)
                                       .ExecuteAsync().Result;
                }
                catch (MsalException ex)
                {
                    Console.WriteLine($"Error getting token for graph: {ex.Message}");
                }
            }
            return result.AccessToken;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("bearer", await GetAccessToken());
        }
    }
}
