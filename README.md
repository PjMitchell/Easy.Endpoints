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

And it will grab all implementations of IEndpoint. IEndpoint is expected to implement a single public method named Handle or HandleAsync
Can extract paramters from route, body, query parameters and from HttpContext

## Sample
Simple endpoint that will be grouped in swagger under Greetings  
```csharp
[EndpointController("Greetings")]  
public class HelloWorldEndpoint : IEndpoint  
{  
    public string Handle()  => "Hello World";
}
```
GET: /Book
Returns a list of books in json format
```csharp
public class GetBookEndpointHandler : IEndpoint
{
    public Task<Book[]> HandleAsync() => Task.FromResult(Array.Empty<Book>());
}
```

POST: /Book
Accepts a Book in the body as Json and returns a Command Result
```csharp
public class PostBookEndpointHandler : IEndpoint
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
public class PostTestResponseEndpoint : IEndpoint
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
public class AnimalEndpointHandler<TAnimal> : IEndpoint where TAnimal : IAnimal
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
public class GetPeopleByIdEndpointHandler : IEndpoint  
{   
    private readonly IPeopleService peopleService

    public GetPeopleByIdEndpointHandler(IPeopleService peopleService)
    {
        this.peopleService = peopleService
    }

    public Task<IEndpointResult> HandleAsync(int id, CancellationToken cancellationToken)  
    {  
        var person = peopleService.AllPeople().SingleOrDefault(p => p.Id == id);  
        if (person is null)  
            return Task.FromResult<IEndpointResult>(new NoContentResult(404));  
        return Task.FromResult<IEndpointResult>(new JsonContentResult<People>(person));  
    }  
}  
```


