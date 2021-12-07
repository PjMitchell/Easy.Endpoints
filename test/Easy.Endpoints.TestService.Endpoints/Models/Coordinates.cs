using System;
using System.Diagnostics.CodeAnalysis;

namespace Easy.Endpoints.TestService.Endpoints
{
    public record Coordinates
    {
        public Coordinates()
        {

        }

        public Coordinates(double lat, double longatude)
        {
            Lat = lat;
            Long = longatude;
        }

        public double Lat { get; init; }
        public double Long { get; init; }
        public override string ToString() => $"{Lat}-{Long}";
    }


    public class CoordinatesParser : IParser<Coordinates>
    {
        public bool TryParse(string input, IFormatProvider formatProvider, [MaybeNullWhen(false)] out Coordinates value)
        {

            var split = input?.Split('-') ?? Array.Empty<string>();
            if (split.Length == 2 && double.TryParse(split[0], out var lat) && double.TryParse(split[1], out var l))
            {
                value = new Coordinates(lat, l);
                return true;
            }
            value = null;
            return false;
        }
    }

}
