namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of update sub-account request.
    /// </summary>
    public class UpdateSubAccountParams : BaseSubAccountParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSubAccountParams"/> class.
        /// </summary>
        /// <param name="subAccountId">The ID of the sub-account.</param>
        public UpdateSubAccountParams(string subAccountId)
        {
            SubAccountId = subAccountId;
        }

        /// <summary>
        /// Gets or sets the ID of the sub-account.
        /// </summary>
        public string SubAccountId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => SubAccountId);
        }
    }
}
