using Repository.DTO;

namespace Repository.Commands.Create;

public sealed record CreateWordRequest(WordDto NewWord) : BaseCreateCommand<WordDto>(NewWord)
{
    public override string ToString()
    {
        string name = $"{nameof(CreateWordRequest)} {NewWord.Id}";
        return name;
    }
}