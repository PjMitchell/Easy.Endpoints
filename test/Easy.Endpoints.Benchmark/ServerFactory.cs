using Easy.Endpoints.Benchmark.Endpoint;
using Easy.Endpoints.TestService.Endpoints;
using Easy.Endpoints.TestService.Endpoints.Books;
using Easy.Endpoints.TestService.Endpoints.People;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Easy.Endpoints.Benchmark
{
    public static class ServerFactory
    {
        public static TestServer CreateMvcServer()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddMvc();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapControllers());
                });

            return new TestServer(builder);
        }

        public static TestServer CreateMinimalApiServer()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddMvc();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("Book", () => GetBookEndpoint.AllBooks().ToArray());
                        //endpoints.MapGet("People", ([FromQuery]string[] firstName, [FromQuery] string[] surname, int? minAge, int? maxAge) =>
                        //{
                        //    return PeopleService.AllPeople().Where(w =>
                        //    {
                        //        if (firstName.Length != 0 && !firstName.Contains(w.FirstName))
                        //            return false;
                        //        if (surname.Length != 0 && !surname.Contains(w.Surname))
                        //            return false;

                        //        if (minAge.HasValue && w.Age < minAge.Value)
                        //            return false;

                        //        if (maxAge.HasValue && w.Age > maxAge.Value)
                        //            return false;

                        //        return true;
                        //    });
                        //});
                        endpoints.MapPost("Book",([FromBody] Book book) => Task.FromResult(new CommandResult { Successful = true, Message = book.Name }));
                    });
                });

            return new TestServer(builder);
        }

        public static TestServer CreateEndpointServer()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddEasyEndpoints(b => b.AddForEndpoint<TestGetEndpoint>()
                    .AddForEndpoint<Test2GetEndpoint>()
                    .AddForEndpoint<GetBookEndpoint>()
                    .AddForEndpoint<PostBookEndpoint>()
                    );
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapEasyEndpoints());
                });

            return new TestServer(builder);
        }

        public static TestServer CreateEndpointServer(Action<EndpointManifestBuilder> manifestBuilderActions)
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddEasyEndpoints(manifestBuilderActions);
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapEasyEndpoints());
                });

            return new TestServer(builder);
        }
    }
}
