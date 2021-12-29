using System;

namespace Easy.Endpoints
{
    /// <summary>
    /// Information on the Parameter Binding
    /// </summary>
    [Flags]
    public enum ParameterBindingFlag : byte
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,
        /// <summary>
        /// No value to Bind
        /// </summary>
        Missing = 1,
        /// <summary>
        /// Bound parameter resulted in an error
        /// </summary>
        Error = 2
    }
}
