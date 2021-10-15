namespace Altinn.ApiClients.Dan.Models.Enums
{
    /// <summary>
    /// Dataset status code ids
    /// </summary>
    public enum StatusCodeId
    {
        /// <summary>
        /// The dataset is available for harvesting
        /// </summary>
        Available = 1,

        /// <summary>
        /// Pending consent
        /// </summary>
        PendingConsent = 2,

        /// <summary>
        /// Access to the dataset was denied
        /// </summary>
        Denied = 3,

        /// <summary>
        /// Access to the dataset has expired
        /// </summary>
        Expired = 4,

        /// <summary>
        /// Access to the dataset value is still pending (for asynchronous data sources)
        /// </summary>
        Waiting = 5,

        /// <summary>
        /// Only used in list views of accreditations, and used when the aggregate dataset status is unknown due to one or more asynchronous datasets in the accreditation that may be expensive to look up 
        /// </summary>
        AggregateUnknown = 6,
    }
}