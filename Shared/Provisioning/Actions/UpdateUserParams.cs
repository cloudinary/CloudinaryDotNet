namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of update user request.
    /// </summary>
    public class UpdateUserParams : BaseUserParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserParams"/> class.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        public UpdateUserParams(string userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// Gets or sets the ID of the user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => UserId);
        }
    }
}
