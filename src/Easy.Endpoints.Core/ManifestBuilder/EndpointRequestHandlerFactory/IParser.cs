using System;
using System.Text.Json;

namespace Easy.Endpoints
{
    internal interface IParser<T>
    {
        bool TryParse(string input,IFormatProvider formatProvider, out T value);
    }

    internal class NumberParser : IParser<byte>,
        IParser<short>, IParser<ushort>,
        IParser<int>, IParser<uint>,
        IParser<long>, IParser<ulong>,
        IParser<float>, IParser<double>, IParser<decimal>
    {
        private static readonly NumberParser Instance = new NumberParser();
        public static bool CanParse(Type type)
        {
            return type == typeof(byte)
                || type == typeof(short) || type == typeof(ushort)
                || type == typeof(int) || type == typeof(uint)
                || type == typeof(long) || type == typeof(ulong)
                || type == typeof(float) || type == typeof(double) || type == typeof(decimal);
        }
        public static IParser<byte> Byte => Instance;
        public static IParser<short> Short => Instance;
        public static IParser<int> Int => Instance;
        public static IParser<long> Long => Instance;
        public static IParser<ushort> UShort => Instance;
        public static IParser<uint> UInt => Instance;
        public static IParser<ulong> ULong => Instance;
        public static IParser<double> Double => Instance;
        public static IParser<float> Float => Instance;
        public static IParser<decimal> Decimal => Instance;

        bool IParser<byte>.TryParse(string input, IFormatProvider formatProvider, out byte value) => byte.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        bool IParser<short>.TryParse(string input, IFormatProvider formatProvider, out short value) => short.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        bool IParser<ushort>.TryParse(string input, IFormatProvider formatProvider, out ushort value) => ushort.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);

        bool IParser<int>.TryParse(string input, IFormatProvider formatProvider, out int value) => int.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        bool IParser<uint>.TryParse(string input, IFormatProvider formatProvider, out uint value) => uint.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);

        bool IParser<long>.TryParse(string input, IFormatProvider formatProvider, out long value) => long.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);
        bool IParser<ulong>.TryParse(string input, IFormatProvider formatProvider, out ulong value) => ulong.TryParse(input, System.Globalization.NumberStyles.Integer, formatProvider, out value);

        bool IParser<double>.TryParse(string input, IFormatProvider formatProvider, out double value) => double.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);
        bool IParser<float>.TryParse(string input, IFormatProvider formatProvider, out float value) => float.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);
        bool IParser<decimal>.TryParse(string input, IFormatProvider formatProvider, out decimal value) => decimal.TryParse(input, System.Globalization.NumberStyles.Float, formatProvider, out value);

    }

    internal class GuidParser : IParser<Guid>
    {
        public static readonly GuidParser Instance = new GuidParser();

        public bool TryParse(string input, IFormatProvider formatProvider, out Guid value) => Guid.TryParse(input, out value);

    }

    internal class BoolParser : IParser<bool>
    {
        public static readonly BoolParser Instance = new BoolParser();

        public bool TryParse(string input, IFormatProvider formatProvider, out bool value) => bool.TryParse(input, out value);

    }

    internal class DateTimeParser : IParser<DateTime>, IParser<DateTimeOffset>, IParser<DateOnly>, IParser<TimeOnly>
    {
        private static readonly DateTimeParser Instance = new DateTimeParser();
        public static bool CanParse(Type type) => type == typeof(DateTime) 
            || type == typeof(DateTimeOffset) 
            || type == typeof(DateOnly)
            || type == typeof(TimeOnly);
        public static IParser<DateTime> DateTime => Instance;
        public static IParser<DateTimeOffset> DateTimeOffset => Instance;
        public static IParser<DateOnly> DateOnly => Instance;
        public static IParser<TimeOnly> TimeOnly => Instance;

        bool IParser<DateTime>.TryParse(string input, IFormatProvider formatProvider, out DateTime value) => System.DateTime.TryParse(input,formatProvider, System.Globalization.DateTimeStyles.None, out value);
        bool IParser<DateTimeOffset>.TryParse(string input, IFormatProvider formatProvider, out DateTimeOffset value) => System.DateTimeOffset.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        bool IParser<DateOnly>.TryParse(string input, IFormatProvider formatProvider, out DateOnly value) => System.DateOnly.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);
        bool IParser<TimeOnly>.TryParse(string input, IFormatProvider formatProvider, out TimeOnly value) => System.TimeOnly.TryParse(input, formatProvider, System.Globalization.DateTimeStyles.None, out value);

    }
}
