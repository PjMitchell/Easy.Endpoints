using System;
using System.Diagnostics.CodeAnalysis;

namespace Easy.Endpoints
{

    /// <summary>
    /// Parse string to object
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Tries to parse string
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="formatProvider">Format provider to use with parser</param>
        /// <param name="value">Parsed result</param>
        /// <returns>True if can parse false if not</returns>
        bool TryParse(string input, IFormatProvider formatProvider, [MaybeNullWhen(false)] out object? value);
    }

    /// <summary>
    /// Parse string to T
    /// </summary>
    /// <typeparam name="T">Type of object to parse</typeparam>
    public interface IParser<T> : IParser
    {
        /// <summary>
        /// Tries to parse string
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="formatProvider">Format provider to use with parser</param>
        /// <param name="value">Parsed result</param>
        /// <returns>True if can parse false if not</returns>
        bool TryParse(string input,IFormatProvider formatProvider, [MaybeNullWhen(false)] out T value);

        bool IParser.TryParse(string input, IFormatProvider formatProvider, [MaybeNullWhen(false)] out object? value)
        {
            if(TryParse(input, formatProvider, out var innerValue))
            {
                value = innerValue;
                return true;
            }
            value = default;
            return false;
        }
    }


}
