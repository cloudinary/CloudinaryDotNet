namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of update user group request.
    /// </summary>
    public class UpdateUserGroupParams : BaseUserGroupParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserGroupParams"/> class.
        /// </summary>
        /// <param name="userGroupId">The ID of the user group to update.</param>
        /// <param name="name">The name of the user group to update.</param>
        public UpdateUserGroupParams(string userGroupId, string name)
            : base(name)
        {
            UserGroupId = userGroupId;
        }

        /// <summary>
        /// Gets or sets the ID of the user.
        /// </summary>
        public string UserGroupId { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            base.Check();
            Utils.ShouldNotBeEmpty(() => UserGroupId);
        }
    }
}
