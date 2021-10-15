using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Altinn.ApiClients.Dan.Models
{
    /// <summary>
    /// Describing a dataset and what values it carries. When used in context of a Accreditation, also includes the timespan of which the dataset is available
    /// </summary>
    public class DataSet
    {
        /// <summary>
        /// The values associated with this dataset
        /// </summary>
        [Required]
        [JsonPropertyName("evidenceValues")]
        public List<DataSetValue> Values { get; set; }
    }
}