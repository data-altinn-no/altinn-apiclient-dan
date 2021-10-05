using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        private readonly JsonSerializerOptions _serializerOptions;
        public DanClientTestController(ILogger<DanClientTestController> logger, IDanClient danClient)
        {
            _logger = logger;
            _danClient = danClient;
            _serializerOptions = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } };
        }

        [HttpGet("{datasetname}/{requestor}/{subject}")]
        [ActionName("direct")]
        public async Task<string> Direct(string datasetname, string requestor, string subject, [FromQuery] Dictionary<string, string> parameters)
        {
            _logger.LogInformation($"GetSynchronousDataset() | datasetname: {datasetname}, subject: {subject}, requestor: {requestor}, parameters: {parameters.ToReadable()}");
            DataSet dataset = await _danClient.GetSynchronousDataset(datasetname, subject, requestor, parameters);
            return JsonSerializer.Serialize(dataset, _serializerOptions);
        }

        [HttpGet("{datasetname}")]
        [ActionName("auth")]
        public async Task<string> Auth(string datasetname, [FromQuery] Dictionary<string, string> parameters)
        {
            _logger.LogInformation($"CreateAsynchronousDatasetRequest().. | datasetname: {datasetname}, parameters: {parameters.ToReadable()}");
            var dataSetRequest = GetDataSetRequest(datasetname, parameters);
            Accreditation accreditation = await _danClient.CreateAsynchronousDatasetRequest(dataSetRequest, "974760673", "991825827");

            _logger.LogInformation($"AccreditationId: {accreditation.AccreditationId}");

            return JsonSerializer.Serialize(accreditation, _serializerOptions);
        }

        [HttpGet("{accreditationId}/{datasetname}")]
        [ActionName("async")]
        public async Task<string> async(string accreditationId, string datasetname)
        {
            _logger.LogInformation($"GetAsynchronousDataset() | input: accreditationId: {accreditationId}, datasetname: {datasetname}");
            DataSet dataSet = await _danClient.GetAsynchronousDataset(accreditationId, datasetname);
            return JsonSerializer.Serialize(dataSet, _serializerOptions);
        }
        
        [HttpGet("{accreditationId}/{datasetname?}")]
        [ActionName("status")]
        public async Task<string> status(string accreditationId, string datasetname)
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
            return JsonSerializer.Serialize(dataSetRequestStatus, _serializerOptions);
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
    
    internal static class DictionaryExtensions
    {
        public static string ToReadable<T, V>(this Dictionary<T, V> d)
        {
            return d == null ? "" : string.Join(" | ", d.Select(a => $"{a.Key}: {a.Value}"));
        }
    }
}