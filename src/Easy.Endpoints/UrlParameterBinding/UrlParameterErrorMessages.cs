namespace Easy.Endpoints
{
    /// <summary>
    /// Common Error Messages for UrlParameters
    /// </summary>
    public static class UrlParameterErrorMessages
    {
        /// <summary>
        /// Error when multiple parameters are found but only one is expected
        /// </summary>
        public const string MultipleParametersFoundError = "Found multiple parameters of {0} when only a single value is expected";

        /// <summary>
        /// Error when could not parse parameter to expected type
        /// </summary>
        public const string CouldNotParseError = "Could not parse {0} as {1}";

        /// <summary>
        /// Invalid Route Parameter
        /// </summary>
        public const string InvalidRouteParameterError = "Invalid route parameter of {0}";
    }
}
