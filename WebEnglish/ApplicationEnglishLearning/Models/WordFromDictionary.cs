using System.ComponentModel.DataAnnotations;

namespace ApplicationEnglishLearning.Models;

public record WordFromDictionary(
    [Required(ErrorMessage = "EnglishWord is required")]
    [MinLength(1, ErrorMessage = "EnglishWord cannot be empty")]
    string EnglishWord,
    [Required(ErrorMessage = "RussianWord is required")]
    [MinLength(1, ErrorMessage = "RussianWord cannot be empty")]
    string RussianWord);


public record WordToTest([Required(ErrorMessage = "EnglishWord is required")]
        [MinLength(1, ErrorMessage = "EnglishWord cannot be empty")]
        string EnglishWord,
        [Required(ErrorMessage = "RussianWord is required")]
        [MinLength(1, ErrorMessage = "RussianWord cannot be empty")]
        string RussianWord);



public record UserToRegister(

    [Required(ErrorMessage = "UserName is required")]
    [MinLength(3, ErrorMessage = "UserName cannot be empty")]
    string UserName,

    [Required(ErrorMessage = "RussianWord is required")]
    [MinLength(3, ErrorMessage = "RussianWord cannot be empty")]
    string PassWord);

