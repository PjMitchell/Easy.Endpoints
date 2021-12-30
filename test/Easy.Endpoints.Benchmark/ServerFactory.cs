using Easy.Endpoints.Benchmark.Endpoint;
using Easy.Endpoints.Benchmark.Mvc;
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
                    services.AddEmptyEasyEndpoints()
                        .WithEndpoint<TestGetEndpoint>()
                        .WithEndpoint<Test2GetEndpoint>()
                        .WithEndpoint<GetBookEndpoint>()
                        .WithEndpoint<Endpoint.GetPeopleEndpoint>()
                        .WithEndpoint<PostBookEndpoint>();
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
