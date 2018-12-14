using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace LinkyLink
{
    public static partial class LinkOperations
    {
        [FunctionName(nameof(GetLinks))]
        public static IActionResult GetLinks(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "links/{vanityUrl}")] HttpRequest req,
            [CosmosDB(
                databaseName: "linkylinkdb",
                collectionName: "linkbundles",
                ConnectionStringSetting = "LinkLinkConnection",
                SqlQuery = "SELECT * FROM linkbundles lb WHERE lb.vanityUrl = {vanityUrl}"
            )] IEnumerable<LinkBundle> documents,
            string vanityUrl,
            ILogger log)
        {
            TrackRequestHeaders(req, $"{nameof(GetLinks)}-HeaderData");
            if (!documents.Any())
            {
                log.LogInformation($"Bundle for {vanityUrl} not found.");

                return new NotFoundResult();
            }

            LinkBundle doc = documents.Single();
            return new OkObjectResult(doc);
        }
    }
}
