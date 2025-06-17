using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using ApplicationEnglishLearning.Models;
using ApplicationEnglishLearning.Validate;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApplicationEnglishLearning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly ConcurrentDictionary<string, string> _summaries = new(
            new Dictionary<string, string>()
        {
            {"Freezing", "Обледенение"}, {"Bracing","Укрепление"}, { "Chilly", "Прохладно" }, { "Cool", "Холод" }, { "Mild", "Мягкий" }, { "Warm", "Теплый" }, { "Balmy", "Нежный" }, { "Hot", "Горячий" }, { "Sweltering", "Изнуряющий" }, { "Scorching", "Палящий" }
        });

        private readonly ILogger<WordsController> _logger;

        public WordsController(ILogger<WordsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "words")]
        public ActionResult<IEnumerable<WordFromDictionary>> Index()
        {
            _logger.LogInformation(message: "HELLO WORLD!");

            return Ok(_summaries.Select(x => new WordFromDictionary(x.Key, x.Value)));
        }


        //[HttpGet(Name = "GetWords")]
        //public IEnumerable<WordFromDictionary> Get()
        //{
        //    return _summaries.Select(x => new WordFromDictionary(x.Key, x.Value));
        //}

        [HttpGet("GetTestedWords/{count}")]
        public IEnumerable<WordToTest> Get(int count)
        {
            var countAllWords = _summaries.Count();

            var trueEngToRus = _summaries.ElementAt(Random.Shared.Next(countAllWords));

            List<WordToTest> words = new List<WordToTest>() { new(trueEngToRus.Key, trueEngToRus.Value) };

            while (words.Count < count)
            { 
                var engToRus = _summaries.ElementAt(Random.Shared.Next(countAllWords));
                if(words.Any(x=> x.RussianWord == engToRus.Value))
                    continue;
                words.Add(new(trueEngToRus.Key, engToRus.Value));
            }

            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(words));

            return words;
        }



        [HttpPut("CreateWord")]
        [ServiceFilter(typeof(ValidateWordFilter))]
        public IActionResult Post([FromBody] WordFromDictionary wordFromDictionary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var resultAdd = _summaries.TryAdd(wordFromDictionary.EnglishWord, wordFromDictionary.RussianWord);


            return resultAdd ? Ok(wordFromDictionary) :
                BadRequest("This wordFromDictionary is contains in dictionary");
        }

        [HttpPut("TestWord")]
        public IActionResult PostTestWord([FromBody] WordToTest wordToTest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_summaries.TryGetValue(wordToTest.EnglishWord, out var expectedWord))
            {
                var tested = wordToTest.RussianWord.ToLower();
                var expected = expectedWord.ToLower();
                var result = expected == tested;

                _logger.LogInformation($"English:{wordToTest.EnglishWord}. Epected:{expected} Tested:{tested}. Result:{result}");
                return Ok(result);
            }
            return BadRequest("This wordFromDictionary is contains in dictionary");
        }
    }
}
