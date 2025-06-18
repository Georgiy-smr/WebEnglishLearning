namespace Repository.DTO;

public record WordDto(string Eng, string Rus, int Id = 0) : BaseDto(Id) ;