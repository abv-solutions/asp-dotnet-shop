using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

// Helper functions

namespace Shop.Client.Services
{
    public class Helpers
    {
        public async Task ErrorResponse(HttpResponseMessage res)
        {
            var body = await res.Content.ReadAsStringAsync();
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body);
            throw new Exception($"{problemDetails.Title} {problemDetails.Detail}");
        }
    }
}
