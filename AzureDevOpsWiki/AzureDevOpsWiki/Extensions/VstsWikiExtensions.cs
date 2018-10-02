using AzureDevOpsWiki.AzureDevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevOpsWiki.Extensions
{
    public static class VstsWikiExtensions
    {
        public static void Sort(this VstsWikiPage page)
        {
            page.PageTitle = ExtractTitle(page.Path);

            if(page != null && page.SubPages != null )
            {
                var sorted = page.SubPages.OrderBy(p => p.Order).ToList();
                page.SubPages.Clear();
                page.SubPages.AddRange(sorted);

                foreach(var subPage in page.SubPages)
                {
                    Sort(subPage);
                }
            }
        }

        public static string ExtractTitle(string gitItemPath)
        {
            var title = gitItemPath;

            if(gitItemPath.Contains("/"))
            {
                title = gitItemPath.Substring(gitItemPath.LastIndexOf("/") + 1);
            }
            return title;
        }
    }
}
