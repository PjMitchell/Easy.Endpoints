namespace Easy.Endpoints
{
    /// <summary>
    /// Information on binding errors
    /// </summary>
    public class BindingError
    {
        /// <summary>
        /// Property that caused the error
        /// </summary>
        public string Property { get; }

        /// <summary>
        /// New instance of Binding error 
        /// </summary>
        /// <param name="property">Property that caused the error</param>
        /// <param name="error">Error message</param>
        public BindingError(string property, string error)
        {
            Property = property;
            Error = error;
        }

        /// <summary>
        /// Error messsage
        /// </summary>
        public string Error { get; }
    }

}
