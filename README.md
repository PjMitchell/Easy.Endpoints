# Easy.Endpoints
Aspnetcore endpoints without the controller

## Setup
Add Easy.Endpoint services
> services.AddRequestEndpoints();
Add Easy.Endpoint to the application builder
>app.UseEndpoints(endpoints =>  
>            {  
>                endpoints.AddEasyEndpoints();  
>            });

And it will grab all implementations of IEndpoint and IEndpointHandler and build routes for them.
