using AdeAuth.Models;
using AdeAuth.Services.AuthServices;
using AdeAuth.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdeAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IAuthService authService, IUserRepository userRepository)
        {
            _logger = logger;
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult<IEnumerable<WeatherForecast>> Get()
        {
            var token = Request.Cookies["AdeAuth"];
            if (token == null)
                return Unauthorized();

            var decodedData = _authService.DecryptData(token);

            var currentUser = _userRepository.GetUserByEmail(decodedData[0]);

            if(currentUser == null)
                return Unauthorized();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

/*        [HttpGet("weather")]
        public ActionResult<IEnumerable<WeatherForecast>> GetWeather()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }*/
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
    }
}
