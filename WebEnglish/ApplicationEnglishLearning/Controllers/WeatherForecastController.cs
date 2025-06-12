using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApplicationEnglishLearning.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class WordsController : ControllerBase
    {
        private static readonly List<string> Summaries = new()
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WordsController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWords")]
        public IEnumerable<string> Get()
        {
            return Summaries;
        }

        [HttpPut("CreateWord")]
        [ServiceFilter(typeof(ValidateWordFilter))]
        public IActionResult Post([FromBody] Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(word);
        }

    }

    public record Word(
        [Required(ErrorMessage = "EnglishWord is required")]
        [MinLength(1, ErrorMessage = "EnglishWord cannot be empty")]
        string EnglishWord,
        [Required(ErrorMessage = "RussianWord is required")]
        [MinLength(1, ErrorMessage = "RussianWord cannot be empty")]
        string RussianWord);

    public class ValidateWordFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("word", out var value) && value is Word word)
            {
                if (word.EnglishWord.Contains('#') || word.RussianWord.Contains('#'))
                {
                    context.Result = new BadRequestObjectResult("Символ '#' запрещён в словах.");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
