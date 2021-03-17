namespace CloudinaryDotNet.Actions
{
    /// <summary>
    /// Parameters of create user request.
    /// </summary>
    public class CreateUserParams : BaseUserParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserParams"/> class.
        /// </summary>
        /// <param name="name">Name of the user.</param>
        /// <param name="email">Email of the user.</param>
        /// <param name="role">The role to assign to the user.</param>
        public CreateUserParams(string name, string email, Role role)
        {
            Name = name;
            Email = email;
            Role = role;
        }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => Name);
            Utils.ShouldNotBeEmpty(() => Email);
            Utils.ShouldBeSpecified(() => Role);
        }
    }
}
