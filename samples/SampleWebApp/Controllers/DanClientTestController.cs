using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SampleWebApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DanClientTestController : ControllerBase
    {
        private readonly ILogger<DanClientTestController> _logger;
        private readonly IDanClient _danClient;
        public DanClientTestController(ILogger<DanClientTestController> logger, IDanClient danClient)
        {
            _logger = logger;
            _danClient = danClient;
        }

        [HttpGet("{datasetname}/{requestor}/{subject}")]
        [ActionName("direct")]
        public async Task<ActionResult> Direct(string datasetname, string requestor, string subject, [FromQuery] Dictionary<string, string> parameters)
        {
            _logger.LogInformation($"GetSynchronousDataset() | datasetname: {datasetname}, subject: {subject}, requestor: {requestor}, parameters: {parameters.ToReadable()}");
            DataSet dataset = await _danClient.GetSynchronousDataset(datasetname, subject, requestor, parameters);
            return Content(dataset.ToHtmlTable(), "text/html; charset=utf-8");
        }

        [HttpGet("{datasetname}")]
        [ActionName("auth")]
        public async Task<ActionResult> Auth(string datasetname, [FromQuery] Dictionary<string, string> parameters)
        {
            _logger.LogInformation($"CreateAsynchronousDatasetRequest().. | datasetname: {datasetname}, parameters: {parameters.ToReadable()}");
            var dataSetRequest = GetDataSetRequest(datasetname, parameters);
            Accreditation accreditation = await _danClient.CreateAsynchronousDatasetRequest(dataSetRequest, "974760673", "991825827");

            _logger.LogInformation($"AccreditationId: {accreditation.AccreditationId}");

            return Content(accreditation.ToHtmlTable(), "text/html; charset=utf-8");
        }

        [HttpGet("{accreditationId}/{datasetname}")]
        [ActionName("async")]
        public async Task<ActionResult> Async(string accreditationId, string datasetname)
        {
            _logger.LogInformation($"GetAsynchronousDataset() | input: accreditationId: {accreditationId}, datasetname: {datasetname}");
            DataSet dataset = await _danClient.GetAsynchronousDataset(accreditationId, datasetname);
            return Content(dataset.ToHtmlTable(), "text/html; charset=utf-8");
        }
        
        [HttpGet("{accreditationId}/{datasetname?}")]
        [ActionName("status")]
        public async Task<ActionResult> Status(string accreditationId, string datasetname)
        {
            _logger.LogInformation($"GetRequestStatus() | input: accreditationId: {accreditationId}, datasetname: {datasetname}");
            List<DataSetRequestStatus> dataSetRequestStatus;
            if (string.IsNullOrEmpty(datasetname))
            {
                dataSetRequestStatus = await _danClient.GetRequestStatus(accreditationId);
            }
            else
            {
                dataSetRequestStatus = await _danClient.GetRequestStatus(accreditationId, datasetname);
            }

            return Content(dataSetRequestStatus.ToHtmlTable(), "text/html; charset=utf-8");
        }
        
        private static DataSetRequest GetDataSetRequest(string datasetname, Dictionary<string, string> parameters)
        {
            DataSetRequest dataSetRequest = new DataSetRequest()
            {
                DataSetName = datasetname,
                Parameters = parameters.Select(kvp => new DataSetParameter()
                {
                    DataSetParamName = kvp.Key, Value = kvp.Value
                }).ToList()
            };

            return dataSetRequest;
        }
    }
    
    internal static class Extensions
    {
        public static string ToReadable<T, V>(this Dictionary<T, V> d)
        {
            return d == null ? "" : string.Join(" | ", d.Select(a => $"{a.Key}: {a.Value}"));
        }

        public static string ToHtmlTable(this DataSet ds)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<table>");
            foreach (var dsv in ds.Values)
            {
                sb.AppendLine($"<tr><th>{dsv.DataSetValueName}:</th><td>{dsv.Value}</td></tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        public static string ToHtmlTable(this Accreditation acr)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<table>");
            sb.AppendLine($"<tr><th>Accreditation-ID:</th><td>{acr.AccreditationId}</td></tr>");
            sb.AppendLine($"<tr><th>Requestor:</th><td>{acr.Requestor}</td></tr>");
            sb.AppendLine($"<tr><th>Subject:</th><td>{acr.Subject}</td></tr>");
            sb.AppendLine($"<tr><th>Issued:</th><td>{acr.Issued}</td></tr>");
            sb.AppendLine($"<tr><th>Dataset(s):</th><td>{string.Join(", ", acr.DataSetCodes)}</td></tr>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        public static string ToHtmlTable(this List<DataSetRequestStatus> drsList)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<table>");
            foreach (var drs in drsList)
            {
                sb.AppendLine($"<tr><th>Dataset:</th><td>{drs.DataSetName}</td></tr>");
                sb.AppendLine($"<tr><th>Status:</th><td>{drs.Status.Description}</td></tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }

    }
}