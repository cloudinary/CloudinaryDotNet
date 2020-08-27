namespace CloudinaryDotNet.Actions
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Parsed result of the sub-accounts listing request.
    /// </summary>
    [DataContract]
    public class ListSubAccountsResult : BaseResult
    {
        /// <summary>
        /// Gets or sets list of the sub-accounts matching the request conditions.
        /// </summary>
        [DataMember(Name = "sub_accounts")]
        public SubAccountResult[] SubAccounts { get; set; }
    }
}
