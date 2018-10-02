using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevOpsWiki.Supports
{
    public class Env
    {
        public class Keys
        {
            public const string PAT = "PAT";
            public const string ORGANIZATION = "ORGANIZATION";
            public const string PROJECTID = "PROJECTID";
            public const string WIKIID = "WIKIID";
            public const string MARKDOWNSERVICE = "MARKDOWNSERVICE";
            public const string SAS = "SAS";
        }

        public static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
