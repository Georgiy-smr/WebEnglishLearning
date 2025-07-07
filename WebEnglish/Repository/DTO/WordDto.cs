using DataBaseOperationHelper.Abstractions;

namespace Repository.DTO;

public record WordDto(string Eng, string Rus, int Id = 0, int UserId = 0) : BaseDto(Id);