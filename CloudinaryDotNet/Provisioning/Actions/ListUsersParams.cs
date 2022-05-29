namespace CloudinaryDotNet.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Parameters of list users request.
    /// </summary>
    public class ListUsersParams : BaseParams
    {
        /// <summary>
        /// Gets or sets whether to limit results to pending users (true), users that are not pending (false),
        /// or all users (undefined, the default).
        /// </summary>
        public bool? Pending { get; set; }

        /// <summary>
        /// Gets or sets a list of up to 100 user IDs. When provided, other parameters are ignored.
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Gets or sets users where the name or email address begins
        /// with the specified case-insensitive string.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        ///  Gets or sets users who have access to the specified account.
        /// </summary>
        public string SubAccountId { get; set; }

        /// <summary>
        ///  Gets or sets users that last logged in the specified range of dates (true),
        ///  users that did not last logged in that range (false), or all users (null).
        /// </summary>
        public bool? LastLogin { get; set; }

        /// <summary>
        ///  Gets or sets last login start date.
        /// </summary>
        public DateTime? From { get; set; }

        /// <summary>
        ///  Gets or sets last login end date.
        /// </summary>
        public DateTime? To { get; set; }

        /// <summary>
        /// Validate object model.
        /// </summary>
        public override void Check()
        {
            // ok
        }

        /// <summary>
        /// Add parameters to the object model dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to be updated with parameters.</param>
        protected override void AddParamsToDictionary(SortedDictionary<string, object> dict)
        {
            if (Pending.HasValue)
            {
                AddParam(dict, "pending", Pending.Value);
            }

            if (UserIds != null && UserIds.Any())
            {
                AddParam(dict, "ids", UserIds);
            }

            if (!string.IsNullOrEmpty(Prefix))
            {
                AddParam(dict, "prefix", Prefix);
            }

            if (!string.IsNullOrEmpty(SubAccountId))
            {
                AddParam(dict, "sub_account_id", SubAccountId);
            }

            if (LastLogin.HasValue)
            {
                AddParam(dict, "last_login", LastLogin);
            }

            if (From.HasValue)
            {
                AddParam(dict, "from", GetIso8601DatetimeFormat(From.Value));
            }

            if (To.HasValue)
            {
                AddParam(dict, "to", GetIso8601DatetimeFormat(To.Value));
            }
        }

        private static string GetIso8601DatetimeFormat(DateTime dateTime) => dateTime.ToString("s", CultureInfo.InvariantCulture);
    }
}
