using System;

namespace Easy.Endpoints
{
    internal static class DefaultParsers
    {

        internal sealed class ByteParser : IParser<byte>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out byte value) => byte.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        internal sealed class ShortParser : IParser<short>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out short value) => short.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        internal sealed class UShortParser : IParser<ushort>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out ushort value) => ushort.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        internal sealed class IntParser : IParser<int>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out int value) => int.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        internal sealed class UIntParser : IParser<uint>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out uint value) => uint.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        internal sealed class LongParser : IParser<long>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out long value) => long.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        internal sealed class ULongParser : IParser<ulong>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out ulong value) => ulong.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        }

        internal sealed class DoubleParser : IParser<double>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out double value) => double.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);
        }

        internal sealed class FloatParser : IParser<float>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out float value) => float.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);
        }
        internal sealed class DecimalParser : IParser<decimal>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out decimal value) => decimal.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);
        }

        internal sealed class GuidParser : IParser<Guid>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out Guid value) => Guid.TryParse(input, out value);
        }

        internal sealed class BoolParser : IParser<bool>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out bool value) => bool.TryParse(input, out value);
        }

        internal sealed class DateTimeParser : IParser<DateTime>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out DateTime value) => System.DateTime.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        }

        internal sealed class DateTimeOffsetParser : IParser<DateTimeOffset>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out DateTimeOffset value) => System.DateTimeOffset.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        }

        internal sealed class DateOnlyParser : IParser<DateOnly>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out DateOnly value) => System.DateOnly.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        }

        internal sealed class TimeOnlyParser : IParser<TimeOnly>
        {
            public bool TryParse(string input, IFormatProvider formatProvider, out TimeOnly value) => System.TimeOnly.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        }

    }
}
