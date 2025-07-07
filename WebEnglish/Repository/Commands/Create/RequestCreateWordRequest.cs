using DataBaseOperationHelper.BaseCommandsOperations.Сreating;
using Repository.DTO;

namespace Repository.Commands.Create;

public sealed record RequestCreateWordRequest(WordDto NewWord) : Create<WordDto>(NewWord)
{
    public override string ToString()
    {
        string name = $"{nameof(RequestCreateWordRequest)} {NewWord.Id}";
        return name;
    }
}