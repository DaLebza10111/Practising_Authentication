using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Practising_Authentication.Model;

namespace Practising_Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly JWTAuthenticationManager _authManager;

        public WeatherForecastController(JWTAuthenticationManager jWTAuthenticationManager, ILogger<WeatherForecastController> logger)
        {
            _jWTAuthenticationManager = jWTAuthenticationManager;
            _logger = logger;
        }

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly JWTAuthenticationManager _jWTAuthenticationManager;

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUsers([FromBody] Customers? customers)
        {
            var token = _jWTAuthenticationManager.Authenticate(customers?.Username, customers?.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}