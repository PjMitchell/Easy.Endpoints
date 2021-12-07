namespace Easy.Endpoints
{
    /// <summary>
    /// Assists with the binding of endpoint parameters to the correct source
    /// </summary>
    public interface IParameterBindingSourceAttribute
    {
        /// <summary>
        /// Source of Parameter
        /// </summary>
        EndpointParameterSource Source { get; }
    }
}
