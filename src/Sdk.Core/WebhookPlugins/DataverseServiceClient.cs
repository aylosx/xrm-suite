namespace Aylos.Xrm.Sdk.Core.WebhookPlugins
{
    using Azure.Core;
    using Azure.Identity;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.PowerPlatform.Dataverse.Client;

    using System;
    using System.Threading.Tasks;

    public class DataverseServiceClient
    {
        public readonly ServiceClient instance;

        public DataverseServiceClient(DataverseServiceConfig config, DefaultAzureCredential credential, IMemoryCache cache, bool useUniqueInstance = true)
        { 
            instance = new ServiceClient(
                tokenProviderFunction: f => GetDataverseToken(config.OrganizationUrl, config.TokenExpiryTime, credential, cache),
                instanceUrl: new Uri(config.OrganizationUrl),
                useUniqueInstance: useUniqueInstance
            );
        }

        private static async Task<string> GetDataverseToken(string dataverseUrl, int dataverseTokenExpiryTime, DefaultAzureCredential credential, IMemoryCache cache)
        {
            var accessToken = await cache.GetOrCreateAsync(dataverseUrl, async (cacheEntry) => {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(dataverseTokenExpiryTime);
                return await credential.GetTokenAsync(new TokenRequestContext(new[] { $"{dataverseUrl}.default" }));
            });
            return accessToken.Token;
        }
    }
}
