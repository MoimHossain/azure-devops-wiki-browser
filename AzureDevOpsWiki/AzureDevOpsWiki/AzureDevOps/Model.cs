using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevOpsWiki.AzureDevOps
{
    public class VstsWikiVersion
    {
        public string Version { get; set; }
    }

    public class VstsWiki
    {
        public string Id { get; set; }
        public List<VstsWikiVersion> Versions { get; set; }
        public string Url { get; set; }
        public string RemoteUrl { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string RepositoryId { get; set; }
        public string MappedPath { get; set; }
    }

    public class VstsWikiCollection
    {
        public List<VstsWiki> Value { get; set; }
        public int Count { get; set; }
    }


    public class VstsWikiPage
    {
        public string PageTitle { get; set; }
        public string Path { get; set; }
        public int Order { get; set; }
        public string GitItemPath { get; set; }
        public string Url { get; set; }
        public string RemoteUrl { get; set; }
        public string Content { get; set; }
        public bool? IsParentPage { get; set; }
        public List<VstsWikiPage> SubPages { get; set; }

        public string Text { get { return this.PageTitle; } }
        public List<VstsWikiPage> Nodes { get { return this.SubPages; } }
    }

    public class MarkdownClass
    {
        public string Markdown { get; set; }
    }

    public class HtmlClass
    {
        public string Html { get; set; }
    }
}
