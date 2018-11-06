using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace fn
{
    public static class AzureSearchFunction
    {
        [FunctionName("Search")]
        public static async Task<IActionResult> Search([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "search/{page}/{term}")]HttpRequest req, ILogger log, [FromRoute]string term, [FromRoute]int page)
        {
            var results = await AzureSearchClient.Search(term, page);

            return new JsonResult(results);
        }
    }
}
