using Repository.DTO;

namespace Repository.Commands.Update;

public sealed record RequestUpdateWordRequest(WordDto Modified) : OperationRequestFromDataBaseRequestUpdateCommand<WordDto>(Modified)
{
    public override string ToString()
    {
        return $"{nameof(RequestUpdateWordRequest)} Id: {Modified.Id} Eng:{Modified.Eng} Rus:{Modified.Rus}";
    }
}