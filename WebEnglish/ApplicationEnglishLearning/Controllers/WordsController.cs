using System.Runtime.InteropServices;
using ApplicationEnglishLearning.Models;
using ApplicationEnglishLearning.Services;
using ApplicationEnglishLearning.Validate;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationEnglishLearning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly ITranslateDictionary<string, string> _translateDictionary;

        private readonly ILogger<WordsController> _logger;

        public WordsController(ILogger<WordsController> logger,
            ITranslateDictionary<string, string> translateDictionary)
        {
            _logger = logger;
            _translateDictionary = translateDictionary;
        }

        [HttpGet(Name = "words")]
        public ActionResult<IEnumerable<WordFromDictionary>> Index()
        {
            return Ok(_translateDictionary.Get().Select(x => new WordFromDictionary(x.Key, x.Value)));
        }


        [HttpGet("GetTestedWords/{count}")]
        public IEnumerable<WordToTest> Get(int count)
        {
            var collectionWords = _translateDictionary.Get();

            var countAllWords = collectionWords.Count();

            var trueEngToRus = collectionWords.ElementAt(Random.Shared.Next(countAllWords));

            List<WordToTest> words = new List<WordToTest>() { new(trueEngToRus.Key, trueEngToRus.Value) };

            while (words.Count < count)
            { 
                var engToRus = collectionWords.ElementAt(Random.Shared.Next(countAllWords));
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


            var resultAdd = _translateDictionary.TryAdd(wordFromDictionary.EnglishWord, wordFromDictionary.RussianWord);


            return resultAdd ? Ok(wordFromDictionary) :
                BadRequest("This wordFromDictionary is contains in dictionary");
        }

        [HttpDelete("DeleteWord")]
        [ServiceFilter(typeof(ValidateWordFilter))]
        public IActionResult Delete([FromBody] WordFromDictionary wordFromDictionary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultAdd = _translateDictionary.TryRemove(wordFromDictionary.EnglishWord, wordFromDictionary.RussianWord);
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
            if (_translateDictionary.TryGetValue(wordToTest.EnglishWord, out var expectedWord))
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
