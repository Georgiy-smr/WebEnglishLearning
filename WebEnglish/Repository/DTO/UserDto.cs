using DataBaseOperationHelper.Abstractions;

namespace Repository.DTO;

public record UserDto(string Name, string HashPass, int Id = 0) : BaseDto(Id);