﻿namespace CloudinaryDotNet
{
    using System.Collections.Generic;

    /// <summary>
    /// Digital signature provider.
    /// </summary>
    public interface ISignProvider
    {
        /// <summary>
        /// Generate digital signature for parameters.
        /// </summary>
        /// <param name="parameters">The parameters to sign.</param>
        /// <returns>Generated signature.</returns>
        string SignParameters(IDictionary<string, object> parameters);

        /// <summary>
        /// Generate digital signature for parameters.
        /// </summary>
        /// <param name="parameters">The parameters to sign.</param>
        /// <param name="signatureVersion">Signature version (1 or 2).</param>
        /// <returns>Generated signature.</returns>
        string SignParameters(IDictionary<string, object> parameters, int signatureVersion);

        /// <summary>
        /// Generate digital signature for part of an URI.
        /// </summary>
        /// <param name="uriPart">The part of an URI to sign.</param>
        /// <param name="isLong">Indicates whether to generate long signature.</param>
        /// <returns>Generated signature.</returns>
        string SignUriPart(string uriPart, bool isLong);

        /// <summary>
        /// Generate digital signature for a string.
        /// </summary>
        /// <param name="toSign">String to sign.</param>
        /// <param name="signatureAlgorithm">Signature algorithm.</param>
        /// <returns>Generated signature.</returns>
        string SignString(string toSign, SignatureAlgorithm signatureAlgorithm);
    }
}
