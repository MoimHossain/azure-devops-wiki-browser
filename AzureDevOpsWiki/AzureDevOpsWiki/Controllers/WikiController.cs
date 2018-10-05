using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AzureDevOpsWiki.AzureDevOps;
using AzureDevOpsWiki.Extensions;
using AzureDevOpsWiki.Supports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureDevOpsWiki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WikiController : ControllerBase
    {
        private readonly AzureDevOpsClient client;
        private string sharedAccessSignature;
        private string pat;
        private string accountName;
        private Guid projectId;
        private Guid repositoryId;
        private string markdownServerUrl;

        public WikiController()
        {
            pat = Env.GetEnvironmentVariable(Env.Keys.PAT);
            accountName = Env.GetEnvironmentVariable(Env.Keys.ORGANIZATION);
            projectId = Guid.Parse(Env.GetEnvironmentVariable(Env.Keys.PROJECTID));
            repositoryId = Guid.Parse(Env.GetEnvironmentVariable(Env.Keys.WIKIID));
            markdownServerUrl = Env.GetEnvironmentVariable(Env.Keys.MARKDOWNSERVICE);
            sharedAccessSignature = Env.GetEnvironmentVariable(Env.Keys.SAS);

            client = new AzureDevOpsClient(accountName, pat);
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetWikiImage(string path, string sas)
        {
            if (!sharedAccessSignature.Equals(sas))
            {
                return new UnauthorizedResult();
            }
            var content = await client.GetWikiImage(projectId, repositoryId, WebUtility.UrlEncode(path));
            return File(content, "image/png");
        }

        [HttpGet("hierarchy")]
        public async Task<ActionResult<VstsWikiPage>> GetWikiHierarchyAsync()
        {
            if (!Request.HasValidApiKey(sharedAccessSignature))
            {
                return new UnauthorizedResult();
            }
            var hierarchy = await client.GetWikiHierarchyAsync(projectId, repositoryId);
            hierarchy.Sort();
            return hierarchy;
        }

        [HttpGet("content")]
        public async Task<ActionResult<VstsWikiPage>> GetWikiPageAsync(string pagePath, string sas)
        {
            if (!Request.HasValidApiKey(sharedAccessSignature))
            {
                return new UnauthorizedResult();
            }
            var articleData = await client.GetWikiPageAsync(projectId, repositoryId, WebUtility.UrlEncode(pagePath), true);

            var htmlData = await new Uri(markdownServerUrl).PostRestAsync<HtmlClass>("convert",
                new MarkdownClass { Markdown = articleData.Content },
                client.GetVstsHeaderFormatter());

            articleData.PageTitle = VstsWikiExtensions.ExtractTitle(articleData.Path);
            articleData.Content = htmlData.Html;

            return articleData;
        }
    }
}