using System.Runtime.Serialization;

namespace Altinn.ApiClients.Dan.Models.Enums
{
    /// <summary>
    /// The type of legal basis, usually. ESPD
    /// </summary>
    public enum LegalBasisType
    {
        /// <summary>
        /// Legal basis is a ESPD document
        /// </summary>
        [EnumMember(Value = "ESPD")] Espd = 1
    }
}