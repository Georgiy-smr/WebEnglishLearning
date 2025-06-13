using ApplicationEnglishLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApplicationEnglishLearning.Validate;

public class ValidateWordFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("word", out var value) && value is WordFromDictionary word)
        {
            if (word.EnglishWord.Contains('#') || word.RussianWord.Contains('#'))
            {
                context.Result = new BadRequestObjectResult("Символ '#' запрещён в словах.");
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}