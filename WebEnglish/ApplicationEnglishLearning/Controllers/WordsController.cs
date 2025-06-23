using System.Linq.Expressions;
using System.Runtime.InteropServices;
using ApplicationEnglishLearning.Models;
using ApplicationEnglishLearning.Services;
using ApplicationEnglishLearning.Validate;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Commands.Create;
using Repository.Commands.Delete;
using Repository.Commands.Read;
using Repository.DTO;
using StatusGeneric;

namespace ApplicationEnglishLearning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<WordsController> _logger;
        private readonly ITranslateDictionary<string, string> _translateDictionary;

        public WordsController(ILogger<WordsController> logger,
            ITranslateDictionary<string, string> translateDictionary, IRepository repository)
        {
            _logger = logger;
            _translateDictionary = translateDictionary;
            _repository = repository;
        }

        [Authorize]
        [HttpGet(Name = "words")]
        public async Task<ActionResult<IEnumerable<WordFromDictionary>>> Index()
        {
            var s = User.Claims;

            int userId = int.Parse(User.FindFirst("userId")?.Value!);

            IStatusGeneric<IEnumerable<WordDto>> resultGet = await _repository.GetItemsAsync(new GetWordsRequest()
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {
                    x => x.User != null & x.User!.Id == userId,
                },
                Includes = new List<Expression<Func<Word, object>>>(),
                Size = 500,
                ZeroStart = 0
            });

            if (resultGet.HasErrors)
                return BadRequest(string.Join(";", resultGet.Errors));

            return Ok(resultGet.Result.Select(x => new WordFromDictionary(x.Eng, x.Rus)));
        }

        [HttpDelete("DeleteWord")]
        [ServiceFilter(typeof(ValidateWordFilter))]
        public async Task<IActionResult> Delete([FromBody] WordFromDictionary wordFromDictionary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IStatusGeneric<IEnumerable<WordDto>> resultGet = await _repository.GetItemsAsync(new GetWordsRequest()
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {
                    x => x.EngWord == wordFromDictionary.EnglishWord,
                    x => x.RusWord == wordFromDictionary.RussianWord
                },
                Includes = new List<Expression<Func<Word, object>>>(),
                Size = 1,
                ZeroStart = 0
            });

            if (resultGet.HasErrors)
                return BadRequest(string.Join(";", resultGet.Errors));

            WordDto wordToRemove = resultGet.Result.Single();
            IStatusGeneric resultRemove = await _repository.DataBaseOperationAsync(new DeleteWordRequest(wordToRemove));

            if (resultRemove.HasErrors)
                return BadRequest(string.Join(";", resultRemove.Errors));

            return Ok(resultRemove.Message);
        }

        [Authorize]
        [HttpPut("CreateWord")]
        [ServiceFilter(typeof(ValidateWordFilter))]
        public async Task<IActionResult> Post([FromBody] WordFromDictionary wordFromDictionary)
        {
            int userId = int.Parse(User.FindFirst("userId")?.Value!);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IStatusGeneric<IEnumerable<WordDto>> resultGet = await _repository.GetItemsAsync(new GetWordsRequest()
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {
                    x => x.EngWord == wordFromDictionary.EnglishWord ||
                         x.RusWord == wordFromDictionary.RussianWord
                },
                Includes = new List<Expression<Func<Word, object>>>(),
                Size = 10,
                ZeroStart = 0
            });

            if (resultGet.IsValid)
                return BadRequest(new { error = "Есть совпадения в словаре." });

            IStatusGeneric resultAdd = await _repository.DataBaseOperationAsync(
                new CreateWordRequest(new WordDto(
                    Eng: wordFromDictionary.EnglishWord,
                    Rus: wordFromDictionary.RussianWord,
                    UserId: userId)));

            if (resultAdd.HasErrors)
                return BadRequest(string.Join(";", resultAdd.Errors));

            return Ok(resultAdd.Message);
        }



        [HttpGet("GetTestedWords/{count}")]
        public async Task<ActionResult<IEnumerable<WordToTest>>> Get(int count)
        {
            IStatusGeneric<IEnumerable<WordDto>> resultGet = await _repository.GetItemsAsync(new GetWordsRequest()
            {
                Filters = new List<Expression<Func<Word, bool>>>(),
                Includes = new List<Expression<Func<Word, object>>>(),
                Size = 500,
                ZeroStart = 0
            });

            if (resultGet.HasErrors)
                return BadRequest(string.Join(";", resultGet.Errors));


            IReadOnlyDictionary<string, string> collectionWords = 
                resultGet.Result.ToDictionary(x => x.Eng, x => x.Rus);

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



        [HttpPut("TestWord")]
        public async Task<IActionResult> PostTestWord([FromBody] WordToTest wordToTest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IStatusGeneric<IEnumerable<WordDto>> resultGet = await _repository.GetItemsAsync(new GetWordsRequest()
            {
                Filters = new List<Expression<Func<Word, bool>>>()
                {
                    x => x.EngWord == wordToTest.EnglishWord,
                    x => x.RusWord == wordToTest.RussianWord
                },
                Includes = new List<Expression<Func<Word, object>>>(),
                Size = 1,
                ZeroStart = 0
            });

            return Ok(resultGet.IsValid);
        }
    }
}
