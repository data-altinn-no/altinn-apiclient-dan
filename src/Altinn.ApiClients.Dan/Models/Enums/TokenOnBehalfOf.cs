using System.Runtime.Serialization;

namespace Altinn.ApiClients.Dan.Models.Enums
{
    public enum TokenOnBehalfOf
    {
        [EnumMember(Value = "owner")]
        Owner,

        [EnumMember(Value = "requestor")]
        Requestor,

        [EnumMember(Value = "subject")]
        Subject
    }
}
