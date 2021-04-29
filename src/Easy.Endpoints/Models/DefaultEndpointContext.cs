using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace Easy.Endpoints
{
    internal class DefaultEndpointContext : EndpointContext
    {
        private readonly HttpContext context;

        public DefaultEndpointContext(HttpContext context)
        {
            this.context = context;
        }

        public override IFeatureCollection Features => context.Features;
        public override HttpRequest Request => context.Request;
        public override HttpResponse Response => context.Response;
        public override ConnectionInfo Connection => context.Connection;
        public override WebSocketManager WebSockets => context.WebSockets;
        public override ClaimsPrincipal User 
        {
            get { return context.User; }
            set { context.User = value; }
        }
        public override IDictionary<object, object?> Items
        {
            get { return context.Items; }
            set { context.Items = value; }
        }
        public override IServiceProvider RequestServices
        {
            get { return context.RequestServices; }
            set { context.RequestServices = value; }
        }
        public override CancellationToken RequestAborted
        {
            get { return context.RequestAborted; }
            set { context.RequestAborted = value; }
        }
        public override string TraceIdentifier
        {
            get { return context.TraceIdentifier; }
            set { context.TraceIdentifier = value; }
        }
        public override ISession Session
        {
            get { return context.Session; }
            set { context.Session = value; }
        }

        public override void Abort() => context.Abort();
    }
}
