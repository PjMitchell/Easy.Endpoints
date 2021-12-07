namespace Easy.Endpoints
{
    /// <summary>
    /// Assists with the binding of endpoint parameters to the correct source
    /// </summary>
    public interface IParameterBindingSourceWithNameAttribute : IParameterBindingSourceAttribute
    {
        /// <summary>
        /// Name of parameter to bind to
        /// </summary>
        string? Name { get; }
    }
}
