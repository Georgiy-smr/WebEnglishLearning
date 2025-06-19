using Repository.DTO;

namespace Repository.Commands.Update;

public sealed record UpdateWordRequest(WordDto Modified) : BaseUpdateCommand<WordDto>(Modified)
{
    public override string ToString()
    {
        return $"{nameof(UpdateWordRequest)} Id: {Modified.Id} Eng:{Modified.Eng} Rus:{Modified.Rus}";
    }
}