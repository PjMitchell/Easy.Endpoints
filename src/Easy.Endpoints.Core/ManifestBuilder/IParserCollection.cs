using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Easy.Endpoints
{
    /// <summary>
    /// Collection of Parsers
    /// </summary>
    public interface IParserCollection
    {
        /// <summary>
        /// If IParser T exists
        /// </summary>
        /// <param name="type">Type of IParser T</param>
        /// <returns>true if parser exists</returns>
        bool HasParser(Type type);
        /// <summary>
        ///  Gets Parser T for type
        /// </summary>
        /// <param name="type">Type of desired IParser T</param>
        /// <param name="parser">Resulting Parser</param>
        /// <returns>True if exists, otherwise false</returns>
        bool TryGetParser(Type type, [NotNullWhen(true)] out IParser? parser);
    }

    internal class DefaultParserCollection : IParserCollection
    {
        private readonly Dictionary<Type, IParser> source;

        public DefaultParserCollection(IEnumerable<IParser> orderedParsers)
        {
            source = new Dictionary<Type, IParser>();
            foreach (var parser in orderedParsers.Where(w => w is not null))
            {
                foreach (var i in parser.GetType().GetInterfaces().Where(r => r.IsGenericType && r.GetGenericTypeDefinition() == typeof(IParser<>)))
                    source[i.GenericTypeArguments[0]] = parser;
            }
        }

        public bool HasParser(Type type) => source.ContainsKey(type);

        public bool TryGetParser(Type type,[NotNullWhen(true)] out IParser? parser) => source.TryGetValue(type, out parser);
    }

    internal class EmptyParserCollection : IParserCollection
    {
        public bool HasParser(Type type) => false;

        public bool TryGetParser(Type type, [NotNullWhen(true)] out IParser? parser)
        {
            parser = default;
            return false;
        }
    }

}
