using System;

namespace Easy.Endpoints
{
    internal interface IParsable<T>
    {
        bool TryParse(string input, out T value);
    }

    internal class IntParser : IParsable<int>
    {
        public static readonly IntParser Instance = new IntParser();

        public bool TryParse(string input, out int value) => int.TryParse(input, out value);
    }

    internal class GuidParser : IParsable<Guid>
    {
        public static readonly GuidParser Instance = new GuidParser();

        public bool TryParse(string input, out Guid value) => Guid.TryParse(input, out value);
    }

    internal class LongParser : IParsable<long>
    {
        public static readonly LongParser Instance = new LongParser();

        public bool TryParse(string input, out long value) => long.TryParse(input, out value);
    }

    internal class BoolParser : IParsable<bool>
    {
        public static readonly BoolParser Instance = new BoolParser();

        public bool TryParse(string input, out bool value) => bool.TryParse(input, out value);
    }

    internal class DoubleParser : IParsable<double>
    {
        public static readonly DoubleParser Instance = new DoubleParser();

        public bool TryParse(string input, out double value) => double.TryParse(input, out value);
    }

    internal class DateTimeParser : IParsable<DateTime>
    {
        public static readonly DateTimeParser Instance = new DateTimeParser();

        public bool TryParse(string input, out DateTime value) => DateTime.TryParse(input, out value);
    }

    internal class DateTimeOffsetParser : IParsable<DateTimeOffset>
    {
        public static readonly DateTimeOffsetParser Instance = new DateTimeOffsetParser();

        public bool TryParse(string input, out DateTimeOffset value) => DateTimeOffset.TryParse(input, out value);
    }
}
