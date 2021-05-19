# Easy.Endpoints
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PjMitchell_Easy.Endpoints&metric=alert_status)](https://sonarcloud.io/dashboard?id=PjMitchell_Easy.Endpoints)  
Aspnetcore endpoints without the controller.  
[Docs](https://github.com/PjMitchell/Easy.Endpoints/wiki)  
[Sample Project](https://github.com/PjMitchell/Easy.Endpoints/tree/master/test/Easy.Endpoints.TestService.Endpoints) 

## Setup
Add Easy.Endpoint services  
```csharp
    services.AddRequestEndpoints();
```
Add Easy.Endpoint to the application builder  
```csharp
app.UseEndpoints(endpoints =>  
  {  
    endpoints.AddEasyEndpoints();  
  });
```

And it will grab all implementations of IEndpoint and IEndpointHandler and build routes for them.

## Sample
Simple endpoint that will be grouped in swagger under Greetings  
```csharp
[EndpointController("Greetings")]  
public class HelloWorldEndpoint : IEndpoint  
{  
    public Task HandleRequestAsync(EndpointContext httpContext)  
    {  
        httpContext.Response.WriteAsync("Hello World");  
        return Task.CompletedTask;  
    }  
}
```
GET: /Book
Returns a list of books in json format
```csharp
public class GetBookEndpointHandler : IJsonResponseEndpointHandler<Book[]>
{
    public Task<Book[]> HandleAsync() => Task.FromResult(Array.Empty<Book>());
}
```

POST: /Book
Accepts a Book in the body as Json and returns a Command Result
```csharp
public class PostBookEndpointHandler : IJsonEndpointHandler<Book, CommandResult>
{
    public PostBookEndpointHandler()
    {            
    }

    public Task<CommandResult> HandleAsync(Book body)
    {
        return Task.FromResult(new CommandResult { Successful = true, Message = "Yay!" });
    }
}
```
POST: /TestOne
Alternatively can declare route and Method
```csharp
[Post("TestOne")]
public class PostTestResponseEndpoint : IJsonBodyEndpointHandler<TestResponsePayload>
{
    public Task HandleAsync(TestResponsePayload body)
    {
        return Task.CompletedTask;
    }
}
```

### Generic handler
Can declare generic handlers that can build routes for types  
POST: /Animal/Cow  
POST: /Animal/Dog  
```csharp
[EndpointController("Animal")]
[KnownTypes("Cow", typeof(Cow))]
[KnownTypes("Dog", typeof(Dog))]
[Post("[controller]/[type]")]
public class AnimalEndpointHandler<TAnimal> : IJsonEndpointHandler<TAnimal, string> where TAnimal : IAnimal
{
    public Task<string> HandleAsync(TAnimal body, CancellationToken cancellationToken)
    {
        return Task.FromResult(body.Says());
    }
}
```
### EndpointResults
Handlers can return IEndpointResult for when more control over the end result is required.
```csharp
[ProducesResponseType(200,Type = typeof(People))]  
[ProducesResponseType(404, Type = typeof(void))]  
[Get("People/{id:int}")]  
public class GetPeopleByIdEndpointHandler : IEndpointResultHandler  
{  
    private readonly IIntIdRouteParser idRouteParser;  
  
    public GetPeopleByIdEndpointHandler(IIntIdRouteParser idRouteParser)  
    {  
        this.idRouteParser = idRouteParser;  
    }   
  
    public Task<IEndpointResult> HandleAsync(CancellationToken cancellationToken)  
    {  
        var id = idRouteParser.GetIdFromRoute();  
        var person = PeopleService.AllPeople().SingleOrDefault(p => p.Id == id);  
        if (person is null)  
            return Task.FromResult<IEndpointResult>(new NoContentResult(404));  
        return Task.FromResult<IEndpointResult>(new JsonContentResult<People>(person));  
    }  
}  
```


