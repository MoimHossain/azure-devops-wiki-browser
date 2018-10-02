

using AzureDevOpsWiki.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsWiki.AzureDevOps
{
    public class AzureDevOpsClient
    {
        private string _accountName;
        private string _personalAccessToken;
        private readonly Uri azureDevOpsUrl = new Uri($"https://dev.azure.com");

        public AzureDevOpsClient(string accountName, string pat)
        {
            this._accountName = accountName;
            this._personalAccessToken = pat;
        }


        public async Task<byte[]> GetWikiImage(Guid projectId, Guid wikiId, string imagePath)
        {
            var path = $"/{_accountName}/{projectId}/_apis/git/repositories/{wikiId}/Items?path={imagePath}&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1";

            var image = await azureDevOpsUrl.GetImageRestAsync(path, GetVstsHeaderFormatter(true));

            return image;
        }

        public async Task<VstsWikiPage> GetWikiPageAsync(Guid projectId, Guid wikiId, string pagePath, bool includeContent)
        {
            var requestPath = $"/{_accountName}/{projectId}/_apis/wiki/wikis/{wikiId}/pages?path={pagePath}&includeContent={includeContent}&api-version=4.1";

            var data = await azureDevOpsUrl.GetRestAsync<VstsWikiPage>(requestPath, GetVstsHeaderFormatter());

            return data;
        }

        public async Task<VstsWikiPage> GetWikiHierarchyAsync(Guid projectId, Guid wikiId)
        {
            var path = "/";
            var recursionLevel = "full";
            var includeContent = false;
            var requestPath = $"/{_accountName}/{projectId}/_apis/wiki/wikis/{wikiId}/pages?path={path}&recursionLevel={recursionLevel}&includeContent={includeContent}&api-version=4.1";

            var data = await azureDevOpsUrl.GetRestAsync<VstsWikiPage>(requestPath, GetVstsHeaderFormatter());

            return data;
        }

        public async Task<VstsWikiCollection> GetWikiListsAsync(Guid projectId)
        {
            var path = $"/{_accountName}/{projectId.ToString()}/_apis/wiki/wikis?api-version=4.1";
            var data = await azureDevOpsUrl.GetRestAsync<VstsWikiCollection>(path, GetVstsHeaderFormatter());

            return data;
        }

        public Action<HttpClient> GetVstsHeaderFormatter(bool contentTypeIsImage = false)
        {
            return new Action<HttpClient>((httpClient) =>
            {
                var credentials =
                Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "", this._personalAccessToken)));
                httpClient.DefaultRequestHeaders.Accept.Clear();
                if (contentTypeIsImage)
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            });
        }

    }
}
