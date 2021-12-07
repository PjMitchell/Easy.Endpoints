using Easy.Endpoints.TestService.Endpoints.Auth;
using Easy.Endpoints.TestService.Endpoints.People;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Easy.Endpoints.TestService.Endpoints
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddEasyEndpoints(o => o.AddParser(new CoordinatesParser()));
            services.AddSwaggerGen();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddTransient<IPeopleService, PeopleService>();
            services.AddAuthentication(TestAuthenticationHandler.Schema).AddScheme<TestAuthOptions, TestAuthenticationHandler>(TestAuthenticationHandler.Schema, o => { });
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger/ui";
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapEasyEndpoints());
        }
    }
}
