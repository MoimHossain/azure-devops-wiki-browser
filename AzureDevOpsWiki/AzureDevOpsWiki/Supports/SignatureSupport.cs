using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevOpsWiki.Supports
{
    public static class SignatureSupport
    {
        public static bool HasValidApiKey(this HttpRequest request, string sharedAccessSignature)
        {
            var sas = request.Headers["x-api-key"];
            return sharedAccessSignature.Equals(sas);
        }
    }
}
