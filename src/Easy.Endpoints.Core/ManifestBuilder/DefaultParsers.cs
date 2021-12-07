using System;
using System.Collections.Generic;
using System.Linq;

namespace Easy.Endpoints
{
    internal static class DefaultParsers
    {
        public static IEnumerable<IParser> GetParsers()
        {
            yield return new ByteParser();
            yield return new UShortParser();
            yield return new ShortParser();
            yield return new UIntParser();
            yield return new IntParser();
            yield return new ULongParser();
            yield return new LongParser();
            yield return new FloatParser();
            yield return new DoubleParser();
            yield return new DecimalParser();
            yield return new BoolParser();
            yield return new DateTimeParser();
            yield return new DateTimeOffsetParser();
            yield return new DateOnlyParser();
            yield return new TimeOnlyParser();
            yield return new GuidParser();
        }

        private sealed class ByteParser : IParser<byte>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out byte value) => byte.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        private sealed class ShortParser : IParser<short>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out short value) => short.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        private sealed class UShortParser : IParser<ushort>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out ushort value) => ushort.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        private sealed class IntParser : IParser<int>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out int value) => int.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        private sealed class UIntParser : IParser<uint>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out uint value) => uint.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        private sealed class LongParser : IParser<long>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out long value) => long.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        private sealed class ULongParser : IParser<ulong>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out ulong value) => ulong.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        private sealed class DoubleParser : IParser<double>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out double value) => double.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);
        }

        private sealed class FloatParser : IParser<float>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out float value) => float.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);
        }
        private sealed class DecimalParser : IParser<decimal>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out decimal value) => decimal.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);
        }

        private sealed class GuidParser : IParser<Guid>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out Guid value) => Guid.TryParse(input, out value);
        }

        private sealed class BoolParser : IParser<bool>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out bool value) => bool.TryParse(input, out value);
        }

        private sealed class DateTimeParser : IParser<DateTime>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out DateTime value) => System.DateTime.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        }

        private sealed class DateTimeOffsetParser : IParser<DateTimeOffset>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out DateTimeOffset value) => System.DateTimeOffset.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        }

        private sealed class DateOnlyParser : IParser<DateOnly>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out DateOnly value) => System.DateOnly.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        }

        private sealed class TimeOnlyParser : IParser<TimeOnly>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out TimeOnly value) => System.TimeOnly.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        }

    }
}
