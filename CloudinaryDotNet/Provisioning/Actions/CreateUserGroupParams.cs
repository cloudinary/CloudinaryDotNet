namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of create user group request.
    /// </summary>
    public class CreateUserGroupParams : BaseUserGroupParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserGroupParams"/> class.
        /// </summary>
        /// <param name="name">The name of the user group.</param>
        public CreateUserGroupParams(string name)
            : base(name)
        {
        }
    }
}
