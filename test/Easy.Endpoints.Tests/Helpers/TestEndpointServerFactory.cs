using Easy.Endpoints.TestService.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace Easy.Endpoints.Tests
{
    public static class TestEndpointServerFactory
    {
        public static TestServer CreateEndpointServer()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();

            return new TestServer(builder);
        }

        public static TestServer CreateEndpointServer(Action<EndpointManifestBuilder> manifestBuilderActions, params IParser[] parsers)
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddEasyEndpoints(o => o.WithJsonSerializer(s => { 
                        s.Converters.Add(new DateOnlyJsonConverter());
                        s.Converters.Add(new TimeOnlyJsonConverter());
                        }).AddParsers(parsers), manifestBuilderActions);
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapEasyEndpoints());
                });

            return new TestServer(builder);
        }
    }

    internal class TimeOnlyJsonConverter : System.Text.Json.Serialization.JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (string.IsNullOrWhiteSpace(s))
                return default;
            return TimeOnly.Parse(s);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("HH:mm:ss"));
        }
    }

    internal class DateOnlyJsonConverter : System.Text.Json.Serialization.JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (string.IsNullOrWhiteSpace(s))
                return default;
            return DateOnly.Parse(s);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}
