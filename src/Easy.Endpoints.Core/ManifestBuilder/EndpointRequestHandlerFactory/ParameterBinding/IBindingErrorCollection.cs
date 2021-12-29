using System.Collections;
using System.Collections.Generic;

namespace Easy.Endpoints
{
    /// <summary>
    /// Collection of Binding errors
    /// </summary>
    public interface IBindingErrorCollection : IEnumerable<BindingError>
    {
        /// <summary>
        /// Add binding error to collection
        /// </summary>
        /// <param name="error">Error to Add</param>
        void Add(BindingError error);
        /// <summary>
        /// Has collection got an error
        /// </summary>
        bool HasErrors { get; }
    }

    internal class BindingErrorCollection : IBindingErrorCollection
    {
        public const string MultipleValueError = "Multiple Values when expecting single";
        private readonly List<BindingError> errors;
        public BindingErrorCollection()
        {
            errors = new List<BindingError>();
        }

        public void Add(BindingError error)
        {
            errors.Add(error);
        }

        public bool HasErrors => errors.Count != 0;

        public IEnumerator<BindingError> GetEnumerator() => errors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => errors.GetEnumerator();
    }


    internal static class BindingErrorCollectionExtensions
    {
        public static void AddCouldNotParseError(this IBindingErrorCollection errors, string param, string value)
        {
            errors.Add(new BindingError(param, $"Could not parse {value}"));
        }

        public static void AddMultipleWhenExpectingSingle(this IBindingErrorCollection errors, string param)
        {

            errors.Add(new BindingError(param, "Multiple Values when expecting single"));
        }

        public static void AddMissingNonNullableValue(this IBindingErrorCollection errors, string param)
        {

            errors.Add(new BindingError(param, "Missing required parameter"));
        }
    }
}
