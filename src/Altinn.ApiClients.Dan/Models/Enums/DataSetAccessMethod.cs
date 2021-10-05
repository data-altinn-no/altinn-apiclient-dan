using System.Runtime.Serialization;

namespace Altinn.ApiClients.Dan.Models.Enums
{
    /// <summary>
    /// How the evidence is accessed
    /// </summary>
    /// <value>How the evidence is accessed</value>
    public enum DataSetAccessMethod
    {
        /// <summary>
        /// Enum Open for open
        /// </summary>
        [EnumMember(Value = "open")] Open = 1,

        /// <summary>
        /// Enum Consent for consent
        /// </summary>
        [EnumMember(Value = "consent")] Consent = 2,

        /// <summary>
        /// Enum LegalBasis for legalBasis
        /// </summary>
        [EnumMember(Value = "legalBasis")] LegalBasis = 3,

        /// <summary>
        /// Enum ConsentOrLegalBasis for consentOrLegalBasis
        /// </summary>
        [EnumMember(Value = "consentOrLegalBasis")]
        ConsentOrLegalBasis = 4
    }
}