namespace Easy.Endpoints
{
    /// <summary>
    /// Result of parameter binding
    /// </summary>
    public readonly struct ParameterBindingResult
    {
        /// <summary>
        /// Infomation on the state of the result, i.e is it missing, or in error
        /// </summary>
        public ParameterBindingIssues State { get; init; }
        /// <summary>
        /// Result value
        /// </summary>
        public object? Result { get; init; }

        /// <summary>
        /// Creates new instance of ParameterBindingResult
        /// </summary>
        /// <param name="result">Value of result</param>
        /// <param name="state">State of the binding</param>
        public ParameterBindingResult(object? result, ParameterBindingIssues state = ParameterBindingIssues.None)
        {
            Result = result;
            State = state;
        }
    }

}
