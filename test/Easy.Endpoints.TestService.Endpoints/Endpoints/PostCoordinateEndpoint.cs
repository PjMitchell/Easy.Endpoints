using Microsoft.Extensions.Logging;
using System;

namespace Easy.Endpoints.TestService.Endpoints
{
    public class PostCoordinateEndpoint : IEndpoint
    {
        private readonly ILogger<PostCoordinateEndpoint> logger;

        public PostCoordinateEndpoint(ILogger<PostCoordinateEndpoint> logger)
        {
            this.logger = logger;
        }

        public void Handle(Coordinates coordinates)
        {
            logger.LogInformation("Posted {Coordinate}", coordinates);
        }
    }
}
