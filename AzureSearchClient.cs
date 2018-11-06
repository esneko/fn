using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Logging;
using fn.Models;

namespace fn
{
    public static class AzureSearchClient
    {
        private static SearchIndexClient GetClient()
        {
            string searchServiceName = Environment.GetEnvironmentVariable("SearchServiceName");
            string queryApiKey = Environment.GetEnvironmentVariable("SearchServiceAdminApiKey");

            return new SearchIndexClient(searchServiceName, "accounts", new SearchCredentials(queryApiKey));
        }

        public static async Task<PagedResult<Account>> Search(string term, int page)
        {
            var searchParams = new SearchParameters();
            searchParams.IncludeTotalResultCount = true;
            searchParams.Skip = (page - 1) * 10;
            searchParams.Top = 10;
            searchParams.OrderBy = new[] { "name" };

            using (var client = GetClient())
            {
                var results = await client.Documents.SearchAsync<Account>(term, searchParams);
                var paged = new PagedResult<Account>();
                paged.CurrentPage = page;
                paged.PageSize = 10;
                paged.RowCount = (int)results.Count;
                paged.PageCount = (int)Math.Ceiling((decimal)paged.RowCount / 10);

                foreach (var result in results.Results)
                {
                    paged.Results.Add(result.Document);
                }

                return paged;
            }
        }

        public static async Task IndexAccount(Account account, ILogger log)
        {
            using (var client = GetClient())
            {
                // TODO: create viewmodel and use automapper
                var azureAccount = new { id = account.Id.ToString(), Name = account.Name, Description = account.Description };
                var batch = IndexBatch.MergeOrUpload(new[] { azureAccount });

                await client.Documents.IndexAsync(batch);
            }
        }

        public static async Task RemoveAccount(int id)
        {
            using (var client = GetClient())
            {
                var batch = IndexBatch.Delete("id", new[] { id.ToString() });

                await client.Documents.IndexAsync(batch);
            }
        }
    }
}
