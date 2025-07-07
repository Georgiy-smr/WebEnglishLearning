using Repository.DTO;
using BaseOperation = DataBaseOperationHelper.BaseCommandsOperations.Updating;
namespace Repository.Commands.Update.Words;

public sealed record RequestUpdateWordRequest(WordDto Modified) : BaseOperation.Update<WordDto>(Modified)
{
    public override string ToString()
    {
        return $"{nameof(RequestUpdateWordRequest)} Id: {Modified.Id} Eng:{Modified.Eng} Rus:{Modified.Rus}";
    }
}