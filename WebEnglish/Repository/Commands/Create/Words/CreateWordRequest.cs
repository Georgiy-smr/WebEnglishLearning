using DataBaseOperationHelper.BaseCommandsOperations.Сreating;
using Repository.DTO;

namespace Repository.Commands.Create.Words;

public sealed record CreateWordRequest(WordDto NewWord) : Create<WordDto>(NewWord)
{
    public override string ToString()
    {
        string name = $"{nameof(CreateWordRequest)} {NewWord.Id}";
        return name;
    }
}