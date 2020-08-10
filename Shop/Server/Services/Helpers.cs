using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// Helper functions

namespace Shop.Server.Services
{
    public class Helpers
    {
        private readonly IHttpContextAccessor _accessor;

        public Helpers(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        // Sends error response
        public JsonResult ErrorResponse(Exception e)
        {
            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Type = $"Database problem",
                Title = $"A database error occurred.",
                Detail = e.Message,
                Instance = _accessor.HttpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", _accessor.HttpContext.TraceIdentifier);

            return new JsonResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentType = "application/problem+json; charset=utf-8"
            };
        }

    }
}
