namespace API.DTOs.Account;

public class GetAccountDto
{
    public Guid Guid { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsUsed { get; set; }
}
