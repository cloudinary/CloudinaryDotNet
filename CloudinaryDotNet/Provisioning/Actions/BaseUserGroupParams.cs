namespace CloudinaryDotNet.Actions
{
    using System.Collections.Generic;

    /// <summary>
    /// Base parameters for user groups modification requests.
    /// </summary>
    public abstract class BaseUserGroupParams : BaseParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserGroupParams"/> class.
        /// </summary>
        /// <param name="name">The name of the user group.</param>
        protected BaseUserGroupParams(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the user group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            Utils.ShouldNotBeEmpty(() => Name);
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            AddParam(dict, "name", Name);
        }
    }
}
