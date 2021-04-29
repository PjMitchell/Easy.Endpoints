# Easy.Endpoints
Aspnetcore endpoints without the controller

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
    public Task HandleRequest(EndpointContext httpContext)  
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
    public Task<Book[]> Handle() => Task.FromResult(Array.Empty<Book>());
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

    public Task<CommandResult> Handle(Book body)
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
    public Task Handle(TestResponsePayload body)
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
    public Task<string> Handle(TAnimal body, CancellationToken cancellationToken)
    {
        return Task.FromResult(body.Says());
    }
}
```
