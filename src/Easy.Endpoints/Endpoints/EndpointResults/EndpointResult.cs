namespace Easy.Endpoints
{
    /// <summary>
    /// Helper for building endpoint results
    /// </summary>
    public static class EndpointResult
    {
        /// <summary>
        /// Completed result, no further modifications to HttpContext
        /// </summary>
        /// <returns>CompletedResult</returns>
        public static IEndpointResult Completed() => new CompletedResult();

        /// <summary>
        /// Returns Status code result only
        /// </summary>
        /// <param name="statusCode">Status code result</param>
        /// <returns>StatusCodeResult with status code</returns>
        public static IEndpointResult StatusCode(int statusCode) => new StatusCodeResult(statusCode);

        /// <summary>
        /// Ok result with json content
        /// </summary>
        /// <typeparam name="T">Type for content</typeparam>
        /// <param name="content">Content</param>
        /// <returns>JsonContentResult with 200 result</returns>
        public static IEndpointResult Ok<T>(T content) => new JsonContentResult<T>(content, 200);

        /// <summary>
        /// Returns 201 response
        /// </summary>
        /// <returns>Returns 201 response</returns>
        public static IEndpointResult NoContent() => new StatusCodeResult(201);
        /// <summary>
        /// Returns 404 response
        /// </summary>
        /// <returns>Returns 404 response</returns>
        public static IEndpointResult NotFound() => new StatusCodeResult(404);


        /// <summary>
        /// Result with Json content and status code
        /// </summary>
        /// <typeparam name="T">Type for content</typeparam>
        /// <param name="content">Content</param>
        /// <param name="statusCode">Status code for result</param>
        /// <returns>JsonContentResult with 200 result</returns>
        public static IEndpointResult Json<T>(T content, int statusCode=200) => new JsonContentResult<T>(content, statusCode);

    }
}
